using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monkey.Sql.AcceptanceTests.Configuration;
using Monkey.Sql.Model;
using TechTalk.SpecFlow;

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
        [Given(@"I have mapped parameter '(.*)' of type '(.*)' to property '(.*)' of type '(.*)' in object command '(.*)'")]
        public async Task GivenIHaveMappedParameterOfTypeToPropertyOfTypeInObject(string paramName, string paramType, string propName, string propType, string objectType)
        {
            var procName = _context["procName"];
            await _applicationExecutor.ExecuteAsync<IRepository>(async repo =>
            {
                var command = new Command() { Name = objectType, IsDynamic = true, IsPrimitive = false, Namespace = "Basic"};
                command.Properties.Add(new ObjectProperty()
                {
                    DeclaringType = command,
                    IsCollection = false,
                    Name = propName,
                    PropertyType = await repo.Query<ObjectType>().FirstAsync(x=>x.Name == "string" && x.IsPrimitive)
                });
                await repo.Add(command);
                await repo.CommitChanges();
            });
        }


    }
}
