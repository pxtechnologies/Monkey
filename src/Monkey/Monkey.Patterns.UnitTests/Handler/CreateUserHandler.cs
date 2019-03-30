using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Monkey.Patterns.UnitTests.WebApi;

namespace Monkey.Patterns.UnitTests.Handler
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserEntity>
    {
        private const string _connectionStringName = "";
        public CreateUserHandler()
        {
            
        }
        public async Task<UserEntity> Execute(CreateUserCommand request)
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString))
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

    public static class ReaderExtensions
    {
        public static int[] GetIndexes(this IDataReader rd, params string[] columns)
        {
            int[] result = new int[rd.FieldCount];

            for (int i = 0; i < result.Length; i++)
            {
                int index = columns.IndexOf(rd.GetName(i));
            }
            
            
            return result;
        }
    }
}