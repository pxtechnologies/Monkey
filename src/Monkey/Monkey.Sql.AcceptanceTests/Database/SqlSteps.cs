using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Monkey.Sql.AcceptanceTests.Configuration;
using TechTalk.SpecFlow;

namespace Monkey.Sql.AcceptanceTests.Database
{
    [Binding]
    public class SqlSteps
    {
        private IApplicationExecutor _applicationExecutor;

        public SqlSteps(IApplicationExecutor applicationExecutor)
        {
            _applicationExecutor = applicationExecutor;
        }

        [Given(@"I have a stored procedure with name '(.*)' in '(.*)' database")]
        public async Task GivenIHaveAStoredProcedureWithNameInDatabase(string proc, string dbName, Table table)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var row in table.Rows) sb.AppendLine(row[0]);

            var connectionString = await _applicationExecutor.ExecuteAsync<IConfiguration, string>(async x => x.GetConnectionString(dbName));

            await new SqlDbTools(connectionString).ReCreateDatabase();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteScalarAsync(sb.ToString());
            }
        }

    }
}
