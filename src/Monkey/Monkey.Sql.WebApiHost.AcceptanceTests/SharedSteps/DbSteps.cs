using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Monkey.Sql.Scripts;
using Monkey.Sql.WebApiHost.AcceptanceTests.Configuration;
using Monkey.Sql.WebApiHost.AcceptanceTests.Services;
using TechTalk.SpecFlow;

namespace Monkey.Sql.WebApiHost.AcceptanceTests.SharedSteps
{
    [Binding]
    public class DbSteps
    {
        private readonly IConfiguration _config;
        private readonly IApplicationExecutor _executor;

        public DbSteps(IConfiguration config, IApplicationExecutor executor)
        {
            _config = config;
            _executor = executor;
        }

        [Given(@"the '(.*)' database is created")]
        public async Task GivenTheDatabaseIsCreated(string dbName)
        {
            await SqlDbTools.ReCreateDatabase(_config.GetConnectionString(dbName));
        }
        [Given(@"Monkey was installed in '(.*)' database")]
        public async Task GivenMonkeyWasInstalledInDatabase(string connectionName)
        {
            await _executor.ExecuteAsync<IScriptManager>(sm => sm.InstallExternal(connectionName));
        }
        [Given(@"I cleared Api by calling '(.*)' on '(.*)' database")]
        public async Task GivenIClearedApiByCallingOnDatabase(string script, string dbName)
        {
            using(var connection = new SqlConnection(_config.GetConnectionString(dbName)))
            {
                await connection.OpenAsync();
                int r = await connection.ExecuteAsync(script);
            }
        }

        [Given(@"I rename the binding with sql statement on '(.*)' database:")]
        [When(@"I publish WebApi on '(.*)' database with sql statement:")]
        [Given(@"I expose the procedure with sql statement on '(.*)' database:")]
        [Given(@"I executed a script against '(.*)' database:")]
        [Given(@"I publish WebApi on '(.*)' database with sql statement:")]
        public async Task GivenIExecutedAScriptAgainstDatabase(string dbName, Table table)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var row in table.Rows) sb.AppendLine(row[0]);

            using (var connection = new SqlConnection(_config.GetConnectionString(dbName)))
            {
                await connection.OpenAsync();
                int r = await connection.ExecuteAsync(sb.ToString());
            }
        }

    }
}
