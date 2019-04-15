using System.Linq;
using System.Threading.Tasks;
using Monkey.Generator;
using Monkey.Logging;
using Monkey.PubSub;
using Monkey.Sql.Extensions;
using Monkey.Sql.Generator;

namespace Monkey.Sql.Services
{
    public interface IServiceMatadataLoader
    {
        Task Load();
    }

    internal class ServiceMatadataLoader : IServiceMatadataLoader
    {
        private ISqlCqrsGenerator _sqlCqrsGenerator;
        private IServiceMetadataProvider _metadataProvider;
        private IDynamicTypePool _dynamicPool;
        private ILogger _logger;
        public ServiceMatadataLoader(IServiceMetadataProvider metadataProvider, 
            ISqlCqrsGenerator sqlCqrsGenerator, 
            IDynamicTypePool dynamicPool, 
            ILogger logger, IEventHub eventHub)
        {
            _metadataProvider = metadataProvider;
            _sqlCqrsGenerator = sqlCqrsGenerator;
            _dynamicPool = dynamicPool;
            _logger = logger;
            _eventHub = eventHub;
        }

        private IEventHub _eventHub;
        public async Task Load()
        {
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
                }
            }
        }
    }

    
}