namespace dbo2
{
    using dbo2;
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

    public class AddUserCommandHandler2 : ICommandHandler<AddUserCommand, AddUserResult>
    {
        private const string _dbName = "Test";
        private const string _procName = "AddUser2";
        private IConfiguration _config;

        public AddUserCommandHandler2(IConfiguration config)
        {
            this._config = config;
        }
        public async Task<AddUserResult> Execute(AddUserCommand cmd)
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
                    command.Parameters.AddWithValue("@birthdate", cmd.Birthdate);

                    using (var rd = await command.ExecuteReaderAsync())
                    {
                        var lz = new Lazy<int[]>(() => rd.GetIndexes("Name", "Id", "BirthDate"), LazyThreadSafetyMode.None);
                        if (await rd.ReadAsync())
                        {
                            var ix = lz.Value;
                            var result = new AddUserResult();
                            if (!(await rd.IsDBNullAsync(ix[0])))
                                result.Name = rd.GetString(ix[0]);
                            if (!(await rd.IsDBNullAsync(ix[1])))
                                result.Id = rd.GetInt32(ix[1]);
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

namespace dbo2
{
    using System;

    public class AddUserCommand
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}

namespace dbo2
{
    using System;

    public class AddUserResult
    {
        public string Name { get; set; }
        public int? Id { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
namespace mrxylpod.WebApi
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Monkey.Cqrs;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using dbo2.WebApi;
    using dbo2;

    public class UserController : ControllerBase
    {
        private IMapper _mapper;
        private ICommandHandler<AddUserCommand, AddUserResult> _addHandler;

        public UserController(ICommandHandler<AddUserCommand, AddUserResult> addHandler, IMapper mapper)
        {
            this._mapper = mapper;
            this._addHandler = addHandler;
        }

        [HttpPost, Route("api/User/{id}/Add")]
        public async Task<AddUserResultResponse> Add(int? id, [FromBody] AddUserRequest request)
        {
            AddUserCommand arg = new AddUserCommand();
            this._mapper.Map(id, arg);
            this._mapper.Map(request, arg);
            AddUserResult result = await _addHandler.Execute(arg);
            return _mapper.Map<AddUserResultResponse>(result);
        }

    }
}

namespace dbo2.WebApi
{
    using System;

    public class AddUserRequest
    {
        public string Name { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}

namespace dbo2.WebApi.Profiles
{
    using System;
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;
    using dbo2;
    using dbo2.WebApi;

    public class AddUserRequestAddUserCommandProfile : Profile
    {
        public AddUserRequestAddUserCommandProfile()
        {
            this.CreateMap<AddUserRequest, AddUserCommand>()
      .ForMember(dst => dst.Id, opt => opt.Ignore());
            this.CreateMap<int?, AddUserCommand>().ForMember(x => x.Id, opt => opt.MapFrom(dst => dst)).ForAllOtherMembers(opt => opt.Ignore());
            this.CreateMap<int, AddUserCommand>().ForMember(x => x.Id, opt => opt.MapFrom(dst => dst)).ForAllOtherMembers(opt => opt.Ignore());

        }
    }
}

namespace dbo2.WebApi
{
    using System;

    public class AddUserResultResponse
    {
        public string Name { get; set; }
        public int? Id { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}

namespace dbo2.WebApi.Profiles
{
    using System;
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;
    using dbo2;
    using dbo2.WebApi;

    public class AddUserCommandAddUserResultResponseProfile : Profile
    {
        public AddUserCommandAddUserResultResponseProfile()
        {
            this.CreateMap<AddUserCommand, AddUserResultResponse>();
        }
    }
}
