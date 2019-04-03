namespace dbo
{
    using Basic;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Monkey.Sql.Extensions;
    using Monkey.Sql;
    using Monkey.Cqrs;
    using Microsoft.Extensions.Configuration;

    public class AddUserCommandHandler : ICommandHandler<AddUserCommand, UserEntity>
    {
        private const string _dbName = "Test";
        private const string _procName = "AddUser";
        private IConfiguration _config;

        public AddUserCommandHandler(IConfiguration config)
        {
            this._config = config;
        }
        public async Task<UserEntity> Execute(AddUserCommand cmd)
        {
            using (var connection = new SqlConnection(_config.GetConnectionString(_dbName)))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = _procName;
                    command.Parameters.AddWithValue("@id", cmd.Id);
                    command.Parameters.AddWithValue("@name", cmd.Name);
                    command.Parameters.AddWithValue("@birthdate", cmd.BirthDate);

                    using (var rd = await command.ExecuteReaderAsync())
                    {
                        var lz = new Lazy<int[]>(() => rd.GetIndexes("Id", "Name", "BirthDate"), LazyThreadSafetyMode.None);
                        if (await rd.ReadAsync())
                        {
                            var ix = lz.Value;
                            var result = new UserEntity();
                            if (!(await rd.IsDBNullAsync(ix[0])))
                                result.Id = rd.GetInt32(ix[0]);
                            if (!(await rd.IsDBNullAsync(ix[1])))
                                result.Name = rd.GetString(ix[1]);
                            if (!(await rd.IsDBNullAsync(ix[2])))
                                result.BirthDate = rd.GetDateTime(ix[2]);
                            return result;
                        }
                        return null;
                    }
                }
            }
        }
    }
}

namespace Basic
{
    using System;

    public class AddUserCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

namespace Basic
{
    using System;

    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
