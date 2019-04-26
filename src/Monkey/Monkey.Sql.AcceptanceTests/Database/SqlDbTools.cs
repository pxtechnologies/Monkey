using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Monkey.Sql.AcceptanceTests.Database
{
    public class SqlDbTools
    {
        private string connection;

        public SqlDbTools(string connection)
        {
            this.connection = connection;
        }

        public static async Task<SqlDbTools> ReCreateDatabase(string connectionString)
        {
            SqlDbTools tools = new SqlDbTools(connectionString);
            await tools.ReCreateDatabase();
            return tools;
        }
        public async Task ReCreateDatabase()
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(connection);
            var dbName = sb.InitialCatalog;
            sb.InitialCatalog = "master";

            using (var masterConnection = new SqlConnection(sb.ConnectionString))
            {
                await masterConnection.OpenAsync();

                var db = await masterConnection.QueryFirstOrDefaultAsync<string>("SELECT name FROM sys.databases WHERE name=@dbName", new { dbName});
                if (db != null)
                    await masterConnection.ExecuteScalarAsync($"ALTER DATABASE [{db}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{db}];");
                await masterConnection.ExecuteScalarAsync("CREATE DATABASE " + dbName);

                db = await masterConnection.QueryFirstOrDefaultAsync<string>("SELECT name FROM sys.databases WHERE name=@dbName", new { dbName });
                if(db == null)
                    throw new InvalidOperationException("Database was not created!");
            }
        }
        
    }
}