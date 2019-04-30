using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Monkey.Logging;
using Monkey.Sql.Model;

namespace Monkey.Sql.Scripts
{
    public interface IScriptManager
    {
        Task InstallExternal(string name);
        Task InstallMonkey();
        Task<string[]> GetMigrations(string name);
        Task<string[]> GetMigrations(SqlConnection connection);
        Task InstallMigrations(SqlConnection connection);
        Task ExecuteSqlScriptFromResourceCatalog(SqlConnection connection, string catalog);
        Task InsertMigration(SqlConnection connection, string scriptName);

        Task ExecuteScript(SqlConnection connection, string scriptName, string sql,
            params SqlParameter[] parameters);
    }

    internal class ScriptManager : IScriptManager
    {
        private const string SqlMonkeyCatalog = "Monkey";
        private const string ScriptTable = "_migrations";
        private const string ProductVersion = "Monkey-1.0.0.0";

        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        public ScriptManager(IConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task InstallExternal(string name)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString(name)))
            {
                await connection.OpenAsync();
                await ExecuteSqlScriptFromResourceCatalog(connection, "External");
            }
        }

        public async Task InstallMonkey()
        {
            var connectionString = _config.GetConnectionString(SqlMonkeyCatalog);

            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(connectionString);
            _logger.Info("Checking or installing Monkey Database {dbName} {server}", sb.InitialCatalog, sb.DataSource);

            var optionsBuilder = new DbContextOptionsBuilder<MonkeyDbContext>();
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable(ScriptTable));
            var dbContext = new MonkeyDbContext(optionsBuilder.Options);
            
            if (dbContext.Database.GetPendingMigrations().Any())
                await dbContext.Database.MigrateAsync();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await ExecuteSqlScriptFromResourceCatalog(connection, SqlMonkeyCatalog);
            }
        }

        public async Task<string[]> GetMigrations(string name = SqlMonkeyCatalog)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString(name)))
            {
                await connection.OpenAsync();
                return await GetMigrations(connection);
            }
        }

        public async Task<string[]> GetMigrations(SqlConnection connection)
        {
            var migrations = new List<string>();
            try
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $"select [MigrationId] from {ScriptTable} where ProductVersion=@p0;";
                    cmd.Parameters.AddWithValue("@p0", ProductVersion);
                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync()) migrations.Add(rd.GetString(0));
                    }
                }

                return migrations.ToArray();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 208 && ex.Class == 16 && ex.Message.Contains(ScriptTable))
                {
                    await InstallMigrations(connection);
                    return new string[0];
                }

                throw;
            }
        }

        public async Task InstallMigrations(SqlConnection connection)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText =
                    $"CREATE TABLE {ScriptTable}([MigrationId] NVARCHAR(300) NOT NULL PRIMARY KEY, ProductVersion nvarchar(64) not null);";
                await cmd.ExecuteScalarAsync();
            }
        }


        public async Task ExecuteSqlScriptFromResourceCatalog(SqlConnection connection, string catalog)
        {
            var assembly = typeof(ScriptManager).Assembly;
            var scripts = assembly
                .GetManifestResourceNames()
                .Where(x => x.StartsWith($"Monkey.Sql.Scripts.{catalog}") && x.EndsWith(".sql"))
                .ToList();
            scripts.Sort();

            var installed = await GetMigrations(connection);
            foreach (var scriptName in scripts.Except(installed))
                using (var sr = new StreamReader(assembly.GetManifestResourceStream(scriptName)))
                {
                    using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        await ExecuteScript(connection, scriptName, await sr.ReadToEndAsync());
                        await InsertMigration(connection, scriptName);
                        tx.Complete();
                    }
                }
        }

        public async Task InsertMigration(SqlConnection connection, string scriptName)
        {
            await ExecuteScript(connection, scriptName,
                $"INSERT INTO {ScriptTable}([MigrationId],[ProductVersion]) VALUES(@p0, @p1)",
                new SqlParameter("@p0", SqlDbType.NVarChar) {Value = scriptName},
                new SqlParameter("@p1", SqlDbType.NVarChar) {Value = ProductVersion});
        }

        public async Task ExecuteScript(SqlConnection connection, string scriptName, string sql,
            params SqlParameter[] parameters)
        {
            try
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    foreach (var p in parameters)
                        cmd.Parameters.Add(p);
                    await cmd.ExecuteScalarAsync();
                }
            }
            catch (SqlException ex)
            {
                throw new MigrationException($"Cannot execute script: '{scriptName}'", ex);
            }
        }
    }
}