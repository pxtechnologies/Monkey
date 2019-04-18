using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Monkey.Generator;
using Monkey.Logging;
using Monkey.PubSub;
using Monkey.Sql.Extensions;
using Monkey.Sql.Generator;
using Monkey.Sql.Model;

namespace Monkey.Sql.Services
{
    public interface IServiceMatadataLoader
    {
        Task Load();
    }

    internal class ServiceMatadataLoader : IServiceMatadataLoader
    {
        private readonly ISqlCqrsGenerator _sqlCqrsGenerator;
        private readonly IServiceMetadataProvider _metadataProvider;
        private readonly IDynamicTypePool _dynamicPool;
        private readonly ILogger _logger;
        private readonly INodeNameProvider _nodeNameProvider;
        private readonly IRepository _repo;
        private readonly IEventHub _eventHub;
        public ServiceMatadataLoader(IServiceMetadataProvider metadataProvider, 
            ISqlCqrsGenerator sqlCqrsGenerator, 
            IDynamicTypePool dynamicPool, 
            ILogger logger, 
            IEventHub eventHub, 
            IRepository repo, 
            INodeNameProvider nodeNameProvider)
        {
            _metadataProvider = metadataProvider;
            _sqlCqrsGenerator = sqlCqrsGenerator;
            _dynamicPool = dynamicPool;
            _logger = logger;
            _eventHub = eventHub;
            _repo = repo;
            _nodeNameProvider = nodeNameProvider;
        }

        
        public async Task Load()
        {
            var dw = AppDomain.CurrentDomain.GetData<DynamicWorkspace>();
            if (dw != null)
            {
                var currentSignature = await _repo.Query<Workspace>()
                    .GetCurrentSignature();
                if (currentSignature != Guid.Empty)
                {
                    if (currentSignature == dw.Signature)
                    {
                        var cw = await _repo.Query<Workspace>()
                            .Where(x => x.Id == dw.CurrentWorkspaceId)
                            .FirstOrDefaultAsync();
                        if (cw != null)
                        {
                            cw.HeartBeat = DateTimeOffset.Now;
                            await _repo.CommitChanges();
                        }
                        return;
                    }
                }
                else
                {
                    // we should disable all controllers
                    return;
                }
            }
            else
                // this is the first run so let's clean up
                await Cleanup();

            // lets see if we need to load last model or compile
            var w = _repo.Query<Workspace>()
                .GetCurrent()
                .ToArray();

            if (!w.Any())
            {
                // This first run or previous runs were not successfull.
                // we expect a record to be created with status Created
                return;
            }
            if (w.Any(x => x.Status != WorkspaceStatus.Created))
            {
                var compilation = _repo.Query<Model.Compilation>()
                    .Where(x => w.Any(y => y.Id == x.WorkspaceId))
                    .WithMonekyHandlerReason()
                    .FirstOrDefault();

                if (compilation != null)
                {
                    await Load(w, compilation);

                    return;
                }
            }
            await CompileAndLoad(w);
        }

        private async Task CompileAndLoad(Workspace[] workspaces)
        {
            var myWorksSpace = await FindOrCreate(workspaces);
            var result = await _sqlCqrsGenerator.Generate(0);
            if (result.Any())
            {
                if (_dynamicPool.All(x => x.SourceUnits.SrcHash != result.SrcHash))
                {
                    _logger.Info("New metadata model loaded from database with {hash} hash.", result.SrcHash);

                    DynamicAssembly assembly = new DynamicAssembly(_eventHub);
                    assembly.AddDefaultReferences();
                    assembly.AppendSourceUnits(result);
                    assembly.Compile();

                    _dynamicPool.AddOrReplace(assembly);
                    _metadataProvider.Clear();
                    _metadataProvider.Discover(assembly.Assembly);

                    var c = await _repo.Query<Model.Compilation>()
                        .FirstAsync(x => x.Hash == assembly.SrcHash);
                    c.Workspace = myWorksSpace;
                    myWorksSpace.Status = WorkspaceStatus.Running;
                    c.LoadedAt = DateTimeOffset.Now;
                    await _repo.CommitChanges();
                }
            }
        }

        private async Task<Workspace> FindOrCreate(Workspace[] workspaces)
        {
            var prv = workspaces.FirstOrDefault(x => x.NodeName == _nodeNameProvider.Name());
            if (prv == null)
            {
                var empty = workspaces.FirstOrDefault(x => x.NodeName == null);
                if(empty != null)
                {
                    empty.NodeName = _nodeNameProvider.Name();
                    await _repo.CommitChanges();
                    AppDomain.CurrentDomain.SetDynamicWorkspace(empty.VersionSignature, empty.Id);
                    return empty;
                }
                else
                {
                    var src = workspaces[0];
                    var clone = new Workspace()
                    {
                        VersionSignature = src.VersionSignature,
                        NodeName = _nodeNameProvider.Name(),
                        Status = WorkspaceStatus.Created
                    };
                    await _repo.Add(clone);
                    await _repo.CommitChanges();
                    AppDomain.CurrentDomain.SetDynamicWorkspace(clone.VersionSignature, clone.Id);
                    return clone;
                }
            }

            return prv;
        }

        private async Task Load(Workspace[] workspaces, Model.Compilation c)
        {
            
            _logger.Info("Old metadata model loaded from database with {hash} hash.", c.Hash);

            DynamicAssembly assembly = new DynamicAssembly(_eventHub);
            assembly.AddDefaultReferences();
            assembly.Load(c.Assembly, c.Hash);

            _dynamicPool.AddOrReplace(assembly);
            _metadataProvider.Clear();
            _metadataProvider.Discover(assembly.Assembly);

            var w = workspaces.FirstOrDefault(x => x.NodeName == _nodeNameProvider.Name());
            if (w == null)
            {
                w = new Workspace()
                {
                    Created = DateTimeOffset.Now,
                    IsDisabled = false,
                    NodeName = _nodeNameProvider.Name(),
                    VersionSignature = workspaces[0].VersionSignature,
                    Status = WorkspaceStatus.Running
                };
                await _repo.Add(w);
                await _repo.CommitChanges();
            }
            else
            {
                w.Status = WorkspaceStatus.Running;
                await _repo.CommitChanges();
            }
            AppDomain.CurrentDomain.SetDynamicWorkspace(w.VersionSignature, w.Id);
        }

        private async Task Cleanup()
        {
            string nodeName = _nodeNameProvider.Name();
            var workspaces = await _repo.Query<Workspace>()
                .Where(x => (x.Status == WorkspaceStatus.Loaded || x.Status == WorkspaceStatus.Running)
                            && x.NodeName == nodeName)
                .ToArrayAsync();
            foreach (var w in workspaces) w.Status = WorkspaceStatus.Compiled;
            await _repo.CommitChanges();
        }
    }

    public interface INodeNameProvider
    {
        string Name();
    }

    internal class NodeNameProvider : INodeNameProvider
    {
        private IConfiguration _config;

        public NodeNameProvider(IConfiguration config)
        {
            _config = config;
        }

        public string Name()
        {
            return _config["NodeName"];
        }
    }

    public static class LetWorkspace
    {
        public static async Task<Guid> GetCurrentSignature(this IQueryable<Workspace> wTable)
        {
            return await wTable.Where(x => !x.IsDisabled &&
                                           (x.Status == WorkspaceStatus.Created ||
                                            x.Status == WorkspaceStatus.Running ||
                                            x.Status == WorkspaceStatus.Compiled))
                .OrderByDescending(x => x.Created)
                .Select(x => x.VersionSignature)
                .FirstOrDefaultAsync();
        }
        public static IQueryable<Workspace> GetCurrent(this IQueryable<Workspace> wTable)
        {
            var signature = wTable.Where(x => !x.IsDisabled &&
                                              (x.Status == WorkspaceStatus.Created ||
                                               x.Status == WorkspaceStatus.Running ||
                                               x.Status == WorkspaceStatus.Compiled))
                .OrderByDescending(x => x.Created)
                .Select(x => x.VersionSignature)
                .FirstOrDefault();
            if (signature == Guid.Empty)
                return new Workspace[0].AsQueryable();

            return wTable.Where(x=>x.VersionSignature == signature);
        }

        public static IQueryable<Model.Compilation> WithMonekyHandlerReason(
            this IQueryable<Model.Compilation> compilations)
        {
            var purpose = AssemblyPurpose.Handlers |
                          AssemblyPurpose.Queries |
                          AssemblyPurpose.Commands |
                          AssemblyPurpose.Results;
            return compilations.Where(x => x.Purpose == purpose);
        }
    }
    public static class AppDomainExtensions
    {
        public static TClass GetData<TClass>(this AppDomain domain) where TClass : class
        {
            var obj = domain.GetData(typeof(TClass).FullName);
            if (obj == null) return null;
            else return (TClass) obj;
        }

        public static void SetDynamicWorkspace(this AppDomain domain,Guid signature, long workspaceId)
        {
            DynamicWorkspace w = new DynamicWorkspace() {CurrentWorkspaceId = workspaceId, Signature = signature};
            domain.SetData(typeof(DynamicWorkspace).FullName, w);
        }
    }
    public class DynamicWorkspace
    {
        public long CurrentWorkspaceId { get; set; }
        public Guid Signature { get; set; }
    }

    

}