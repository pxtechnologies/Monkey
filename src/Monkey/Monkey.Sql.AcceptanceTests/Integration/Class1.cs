using NUnit.Framework;

namespace dbo
{
    using dbo;
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Monkey.Sql.Extensions;
    using Monkey.Sql;
    using Monkey.Cqrs;
    using Microsoft.Extensions.Configuration;
    public static class ConfigurationFactory
    {
        public static IConfigurationRoot Load()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables()
                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            return config;
        }
    }
    [TestFixture]
    public class AddProductCommandHandlerTests
    {
        [Test]
        public async Task Foo()
        {
            AddProductCommandHandler h = new AddProductCommandHandler(ConfigurationFactory.Load());

            var result = await h.Execute(new AddProductCommand());
        }
    }

    public class AddProductCommandHandler : ICommandHandler<AddProductCommand, AddProductResult>
    {
        private const string _dbName = "Test";
        private const string _procName = "AddProduct";
        private IConfiguration _config;

        public AddProductCommandHandler(IConfiguration config)
        {
            this._config = config;
        }
        public async Task<AddProductResult> Execute(AddProductCommand cmd)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString(_dbName)))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = _procName;

                    var parameters = command.Parameters;

                    if (cmd.Name == null) parameters.AddWithValue("@name", DBNull.Value);
                    else parameters.AddWithValue("@name", cmd.Name);

                    if (cmd.Number == null) parameters.AddWithValue("@number", DBNull.Value);
                    else parameters.AddWithValue("@number", cmd.Number);

                    using (var rd = await command.ExecuteReaderAsync())
                    {
                        var lz = new Lazy<int[]>(() => rd.GetIndexes("Name", "Number"), LazyThreadSafetyMode.None);
                        if (await rd.ReadAsync())
                        {
                            var ix = lz.Value;
                            var result = new AddProductResult();
                            if (!(await rd.IsDBNullAsync(ix[0])))
                                result.Name = rd.GetString(ix[0]);
                            if (!(await rd.IsDBNullAsync(ix[1])))
                                result.Number = rd.GetInt32(ix[1]);
                            return result;
                        }
                        return null;
                    }
                }
            }
        }
    }
}

namespace dbo
{
    using System;

    public class AddProductCommand
    {
        public string Name { get; set; }
        public int? Number { get; set; }
    }
}

namespace dbo
{
    using System;

    public class AddProductResult
    {
        public string Name { get; set; }
        public int? Number { get; set; }
    }
}

