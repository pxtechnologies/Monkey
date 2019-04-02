using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monkey.Sql.AcceptanceTests.Configuration;
using Monkey.Sql.Model;
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
                await repo.Add(new ProcedureDescriptor() { ConnectionName = dbName, Name = procedureName, Schema = schema });
                await repo.CommitChanges();
            });
            _context["procName"] = procedureName;
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
        public void WhenACommandhandlerIsGenerated()
        {
            ScenarioContext.Current.Pending();
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
