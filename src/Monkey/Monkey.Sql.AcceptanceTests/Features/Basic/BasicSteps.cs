using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Monkey.Compilation;
using Monkey.Cqrs;
using Monkey.Generator;
using Monkey.SimpleInjector;
using Monkey.Sql.AcceptanceTests.Configuration;
using Monkey.Sql.Generator;
using Monkey.Sql.Model;
using Monkey.Sql.SimpleInjector;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Monkey.Sql.AcceptanceTests.Features.Basic
{
    [Binding]
    public class BasicSteps
    {
        private IApplicationExecutor _applicationExecutor;
        private readonly ScenarioContext _context;

        public BasicSteps(IApplicationExecutor applicationExecutor, ScenarioContext context)
        {
            _applicationExecutor = applicationExecutor;
            _context = context;
        }

        [Given(@"I have mapped '(.*)' procedure from '(.*)' database in apidatabase in schema '(.*)'")]
        public async Task GivenIHaveMappedProcedureFromDatabaseInApidatabase(string procedureName, string dbName, string schema)
        {
            await _applicationExecutor.ExecuteAsync<IRepository>(async repo =>
            {
                var proc = new ProcedureDescriptor() { ConnectionName = dbName, Name = procedureName, Schema = schema };
                await repo.Add(proc);
                

                await repo.CommitChanges();
            });
            _context["procName"] = procedureName;
        }
        [Given(@"I bind that procedure")]
        public async Task GivenIBindThatProcedure()
        {
            var procName = _context["procName"].ToString();
            var commadId = (long)_context["commandId"];
            var resultId = (long) _context["resultId"];

            await _applicationExecutor.ExecuteAsync<IRepository>(async repo =>
                {
                var procBinding = new ProcedureBinding()
                {
                    IsResultCollection = false,
                    Mode = Mode.Command,
                    Name = "AddUser",
                    Procedure = await repo.Query<ProcedureDescriptor>()
                        .FirstAsync(x=>x.Name == procName),
                    RequestId = commadId,
                    ResultId = resultId
                };
                await repo.Add(procBinding);
                await repo.CommitChanges();
                });
        }

        [Given(@"I have mapped resultset '(.*)'")]
        public async Task GivenIHaveMappedResultsetToObject(string resultObjectType, Table table)
        {
            var mappings = table.CreateSet<ResultSet>();
            var procName = _context["procName"].ToString();
            await _applicationExecutor.ExecuteAsync<IRepository>(async repo =>
            {
                Result r = new Result();
                r.Name = resultObjectType;
                r.IsDynamic = true;
                r.Namespace = "Basic";
                await repo.Add(r);
                _context["resultId"] = r.Id;

                foreach (var m in mappings)
                {
                    var property = new ObjectProperty()
                    {
                        DeclaringType = r,
                        IsCollection = false,
                        Name = m.PropertyName,
                        PropertyType = await repo.Query<ObjectType>()
                            .FirstAsync(x => x.Alias == m.PropertyType && x.IsPrimitive)
                    };
                    r.Properties.Add(property);

                    ProcedureResultColumnBinding binding = new ProcedureResultColumnBinding();
                    binding.ObjectProperty = property;
                    binding.ResultColumnColumn = await repo.Query<ProcedureResultColumn>()
                        .FirstOrDefaultAsync(x => x.Name == m.SqlColumnName);

                    if (binding.ResultColumnColumn == null)
                    {
                        binding.ResultColumnColumn = new ProcedureResultColumn()
                        {
                            Name = m.SqlColumnName,
                            Procedure = await repo.Query<ProcedureDescriptor>().FirstAsync(x => x.Name == procName),
                            Type = m.SqlColumnType
                        };
                    }

                    await repo.Add(binding);
                }

                await repo.CommitChanges();
            });
        }

        [When(@"a commandhandler is generated")]
        public async Task WhenACommandhandlerIsGenerated()
        {
            var result = await this._applicationExecutor.ExecuteAsync<SqlCqrsGenerator,IEnumerable<SourceUnit>>( x => x.Generate() );
            
            DynamicAssembly assembly = new DynamicAssembly();
            assembly.AppendSourceUnits(result);
            assembly.AddReferenceFromType(typeof(ICommandHandler<,>));
            assembly.AddReferenceFromType<SqlPackage>();
            assembly.AddReferenceFromType<MonkeyPackage>();
            assembly.AddReferenceFromType<IRepository>();
            assembly.AddReferenceFromType<SqlParameterCollection>();
            assembly.AddReferenceFromType<IConfiguration>();

            TypeCompiler compiler = new TypeCompiler();
            _context["assembly"] = assembly.Compile(compiler);
        }
        [When(@"It is executed with command '(.*)'")]
        public void ThenOnceItIsExecutedWithCommand(string p0)
        {
            ScenarioContext.Current.Pending();
        }


        [Given(@"I have mapped parameters to command '(.*)'")]
        public async Task GivenIHaveMappedParametersToCommand(string commandName, Table table)
        {
            var mappings = table.CreateSet<CommandParameter>();
            var procName = _context["procName"].ToString();

            await _applicationExecutor.ExecuteAsync<IRepository>(async repo =>
            {
                var proc = await repo.Query<ProcedureDescriptor>().FirstAsync(x => x.Name == procName);
                var command = new Command()
                {
                    Name = commandName,
                    IsDynamic = true,
                    IsPrimitive = false,
                    Namespace = "Basic"
                };
                await repo.Add(command);
                _context["commandId"] = command.Id;
                foreach (var m in mappings)
                {
                    
                    var property = new ObjectProperty()
                    {
                        DeclaringType = command,
                        IsCollection = false,
                        Name = m.PropertyName,
                        PropertyType = await repo.Query<ObjectType>()
                            .FirstAsync(x => x.Alias == m.PropertyType && x.IsPrimitive)
                    };
                    command.Properties.Add(property);
                    
                    await repo.CommitChanges();

                    var binding = new ProcedureParameterBinding();
                    binding.ObjectProperty = property;
                    binding.Parameter = await repo.Query<ProcedureParameterDescriptor>()
                        .FirstOrDefaultAsync(x => x.Procedure.Name == procName && x.Name == m.SqlParameterName);

                    if (binding.Parameter == null)
                    {
                        binding.Parameter = new ProcedureParameterDescriptor()
                        {
                            Name = m.SqlParameterName,
                            Procedure = proc,
                            Type = m.SqlType
                        };
                    }
                    
                    await repo.Add(binding);
                    
                }
                await repo.CommitChanges();
            });

        }

        
        
    }

    public class CommandParameter
    {
        public string SqlParameterName { get; set; }
        public string SqlType { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        
    }
    public class ResultSet
    {
        public string SqlColumnName { get; set; }
        public string SqlColumnType { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        
        
    }
}
