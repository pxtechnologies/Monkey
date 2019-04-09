using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Monkey.Cqrs;

namespace Monkey.Patterns.UnitTests.WebApi
{
    public class UserResource
    {
        public string Name { get; set; }
    }

    public class CreateUserRequest
    {
        public string Name { get; set; }
    }

    public class UpdateUserRequest
    {
    }

    public class UserEntity
    {
        public string Name { get; set; }
    }

    public class ActivateUserCommand
    {
        public Guid Id { get; set; }
        public DateTime When { get; set; }
        public string Name { get; set; }
    }
    public class UpdateUserCommand
    {
        public Guid Id { get; set; }
        public DateTime When { get; set; }
        public string Name { get; set; }
    }

    public class ActivateUserHandler : ICommandHandler<ActivateUserCommand, UserEntity>
    {
        public async Task<UserEntity> Execute(ActivateUserCommand cmd)
        {
            throw new NotImplementedException();
        }
    }
    public class UpdateUserHandler : ICommandHandler<UpdateUserCommand, UserEntity>
    {
        public async Task<UserEntity> Execute(UpdateUserCommand cmd)
        {
            throw new NotImplementedException();
        }
    }
    public class CreateUserCommand
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class GetActiveUsersFromRequest
    {
        public bool IsActive { get; set; }
    }

    public class GetActiveUsersFromQuery
    {
        public bool IsActive { get; set; }
    }

    public class CreteUserProfile : Profile
    {
        public CreteUserProfile()
        {
            this.CreateMap<Guid, CreateUserCommand>().ForMember(x => x.Name, opt => opt.MapFrom(dst => dst));
        }
    }
    public class GetUserQuery
    {
    }

    public class GetUserByIdQuery
    {
        public Guid Id { get; set; }
    }

    public class GetUserByNameQuery
    {
        public string Name { get; set; }
    }

    public class UsersController : ControllerBase
    {
        private IRequestHandler<CreateUserCommand, UserEntity> _postHandler;
        private IRequestHandler<UpdateUserCommand, UserEntity> _putHandler;
        private IRequestHandler<GetUserQuery, UserResource[]> _getUsers;
        private IRequestHandler<GetUserByIdQuery, UserResource> _getUserById;
        private IRequestHandler<GetUserByNameQuery, UserResource> _getUserByName;
        private IRequestHandler<GetActiveUsersFromQuery, UserResource[]> _getActiveUsersFrom;

        private IMapper _mapper;
        
        public UsersController(IRequestHandler<CreateUserCommand, UserEntity> postHandler,
            IRequestHandler<UpdateUserCommand, UserEntity> handler,
            IRequestHandler<GetUserQuery, UserResource[]> users,
            IRequestHandler<GetUserByIdQuery, UserResource> getUserById,
            IRequestHandler<GetUserByNameQuery, UserResource> getUserByName,
            IRequestHandler<GetActiveUsersFromQuery, UserResource[]> getActiveUsersFrom, 
            IMapper mapper)
        {
            _postHandler = postHandler;
            _putHandler = handler;
            _getUsers = users;
            _getUserById = getUserById;
            _getUserByName = getUserByName;
            _getActiveUsersFrom = getActiveUsersFrom;
            _mapper = mapper;
        }

        public async Task<UserResource> Post(CreateUserRequest request)
        {
            var cmd = _mapper.Map<CreateUserCommand>(request); // Optional if request != cmd
            var result = await _postHandler.Execute(cmd); // Must
            return _mapper.Map<UserResource>(result); // Optional if result != response
        }

        public async Task<UserResource> Put(Guid id, UpdateUserRequest request)
        {
            UpdateUserCommand cmd = new UpdateUserCommand();
            _mapper.Map(id, cmd);
            _mapper.Map(request, cmd);

            var result = await _putHandler.Execute(cmd);

            return _mapper.Map<UserResource>(result);
        }

        public async Task<UserResource[]> Get()
        {
            var result = await _getUsers.Execute(new GetUserQuery());

            return result.Select(_mapper.Map<UserResource>).ToArray();
        }

        public async Task<UserResource> Get(Guid id)
        {
            var result = await _getUserById.Execute(_mapper.Map<GetUserByIdQuery>(id));

            return _mapper.Map<UserResource>(result);
        }

        public async Task<UserResource> GetByName(string name)
        {
            var result = await _getUserByName.Execute(_mapper.Map<GetUserByNameQuery>(name));

            return _mapper.Map<UserResource>(result);
        }

        public async Task<UserResource[]> GetActiveUsersFrom(GetActiveUsersFromRequest query)
        {
            var result = await _getActiveUsersFrom.Execute(_mapper.Map<GetActiveUsersFromQuery>(query));

            return result.Select(_mapper.Map<UserResource>).ToArray();
        }
    }
}