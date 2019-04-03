using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Monkey.Patterns.UnitTests.WebApi;
using Monkey.Sql.Extensions;

namespace Monkey.Patterns.UnitTests.Handler
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserEntity>
    {
        private const string _connectionStringName = "";
        private IConfiguration _config;
        public CreateUserHandler()
        {
            
        }
        public async Task<UserEntity> Execute(CreateUserCommand request)
        {
            using (var connection =
                new SqlConnection(_config.GetConnectionString(_connectionStringName)))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(nameof(request.Name), request.Name);

                    using (var rd = cmd.ExecuteReader())
                    {
                        var lz = new Lazy<int[]>(() => rd.GetIndexes(), LazyThreadSafetyMode.None);
                        if (rd.Read())
                        {
                            var ix = lz.Value;
                            UserEntity row = new UserEntity();

                            row.Name = rd.GetString(ix[0]);




                            return row;
                        }

                    }
                }
            }

            return null;
        }
       
    }
}