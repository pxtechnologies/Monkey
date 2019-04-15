using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Monkey.Compilation;
using Monkey.Cqrs;
using Monkey.Generator;
using Monkey.PubSub;
using Monkey.SimpleInjector;
using Monkey.Sql.AcceptanceTests.Configuration;
using Monkey.Sql.Extensions;
using Monkey.Sql.Generator;
using Monkey.Sql.Model;
using Monkey.Sql.SimpleInjector;
using Newtonsoft.Json;
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

                byte j = 1;
                foreach (var m in mappings)
                {
                    var property = new ObjectProperty()
                    {
                        DeclaringType = r,
                        IsCollection = false,
                        Name = m.PropertyName,
                        PropertyType = await repo.Query<ObjectType>()
                            .FirstAsync(x => (x.Alias == m.PropertyType || x.Name==m.PropertyType ) && x.IsPrimitive)
                    };
                    r.Properties.Add(property);

                    ProcedureResultColumnBinding binding = new ProcedureResultColumnBinding();
                    binding.Property = property;
                    binding.ResultColumn = await repo.Query<ProcedureResultColumn>()
                        .FirstOrDefaultAsync(x => x.Name == m.SqlColumnName);

                    if (binding.ResultColumn == null)
                    {
                        binding.ResultColumn = new ProcedureResultColumn()
                        {
                            Name = m.SqlColumnName,
                            Procedure = await repo.Query<ProcedureDescriptor>().FirstAsync(x => x.Name == procName),
                            Type = m.SqlColumnType,
                            Order = j++
                        };
                    }

                    await repo.Add(binding);
                }

                await repo.CommitChanges();
            });
        }
        
        

        [When(@"a commandhandler is generated as '(.*)'")]
        public async Task WhenACommandhandlerIsGenerated(string handlerName)
        {
            var result = await this._applicationExecutor.ExecuteAsync<ISqlCqrsGenerator,SourceUnitCollection>( x => x.Generate(0) );
            
            DynamicAssembly assembly = new DynamicAssembly(NSubstitute.Substitute.For<IEventHub>());
            assembly.AppendSourceUnits(result);
            assembly.AddDefaultReferences();
            
            TypeCompiler compiler = new TypeCompiler();
            _context["assembly"] = assembly.Compile(compiler);

            assembly.Assembly.GetTypes().Should().Contain(x => x.Name == handlerName);
        }
        [When(@"It is executed with command '(.*)'")]
        public async Task ThenOnceItIsExecutedWithCommand(string json)
        {
            DynamicAssembly assembly = (DynamicAssembly)_context["assembly"];
            var type = assembly.Load("Basic", "AddUserCommand");
            dynamic arg = JsonConvert.DeserializeObject(json, type);

            var commandHandlerType = assembly.Load("dbo", "AddUserCommandHandler");
            await _applicationExecutor.ExecuteAsync<IServiceProvider>(async conteiner =>
            {
                dynamic commandHandler = conteiner.GetService(commandHandlerType);
                _context["result"] = await commandHandler.Execute(arg);
            });
        }
        [Then(@"result is: '(.*)'")]
        public void ThenResultIs(string json)
        {
            DynamicAssembly assembly = (DynamicAssembly)_context["assembly"];
            var type = assembly.Load("Basic", "UserEntity");
            object expected = JsonConvert.DeserializeObject(json, type);
            object actual = _context["result"];

            actual.Should().BeEquivalentTo(expected);
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
                byte j = 1;
                foreach (var m in mappings)
                {
                    
                    var property = new ObjectProperty()
                    {
                        DeclaringType = command,
                        IsCollection = false,
                        Name = m.PropertyName,
                        PropertyType = await repo.Query<ObjectType>()
                            .FirstAsync(x => (x.Alias == m.PropertyType || x.Name==m.PropertyType ) && x.IsPrimitive)
                    };
                    command.Properties.Add(property);
                    
                    await repo.CommitChanges();

                    var binding = new ProcedureParameterBinding();
                    binding.Property = property;
                    binding.Parameter = await repo.Query<ProcedureParameterDescriptor>()
                        .FirstOrDefaultAsync(x => x.Procedure.Name == procName && x.Name == m.SqlParameterName);

                    if (binding.Parameter == null)
                    {
                        binding.Parameter = new ProcedureParameterDescriptor()
                        {
                            Name = m.SqlParameterName,
                            Procedure = proc,
                            Type = m.SqlType,
                            Order = j++
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
