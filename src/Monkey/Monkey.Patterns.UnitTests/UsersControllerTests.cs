using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Monkey.Patterns.UnitTests.WebApi;
using NSubstitute;
using Xunit;

namespace Monkey.Patterns.UnitTests
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserRequest, CreateUserCommand>();
            CreateMap<Guid, CreateUserCommand>().ForMember(x => x.Id, opt => opt.MapFrom(dst => dst));
            CreateMap<UpdateUserRequest, UpdateUserCommand>();

            CreateMap<Guid, GetUserByIdQuery>().ForMember(x => x.Id, opt => opt.MapFrom(src => src));
            CreateMap<string, GetUserByNameQuery>().ForMember(dst => dst.Name, opt => opt.MapFrom(src => src));
            CreateMap<GetActiveUsersFromRequest, GetActiveUsersFromQuery>();
            CreateMap<UserEntity, UserResource>();
        }
    }
    public class UsersControllerTests : TestFor<UsersController>
    {
        private IRequestHandler<CreateUserCommand, UserEntity> _postHandler;
        private IRequestHandler<UpdateUserCommand, UserEntity> _putHandler;
        private IRequestHandler<GetUserQuery, UserResource[]> _getUsers;
        private IRequestHandler<GetUserByIdQuery, UserResource> _getUserById;
        private IRequestHandler<GetUserByNameQuery, UserResource> _getUserByName;
        private IRequestHandler<GetActiveUsersFromQuery, UserResource[]> _getActiveUsersFrom;
        private IMapper _mapper;

        
        public UsersControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
                
            });
            config.CompileMappings();
            _mapper = config.CreateMapper();

        }

        protected override UsersController CreateSut()
        {
            return new UsersController(_postHandler, _putHandler, _getUsers, _getUserById, _getUserByName, _getActiveUsersFrom, _mapper);
        }

        [Fact]
        public async Task CheckMapping()
        {
            var req = new CreateUserRequest() { Name = "John" };
            CreateUserCommand cmd = new CreateUserCommand();
            var id = Guid.NewGuid();
            _mapper.Map(id, cmd);
            _mapper.Map(req, cmd);

            cmd.Name.Should().Be("John");
            cmd.Id.Should().Be(id);
        }
        [Fact]
        public async Task ConfigureAndPost()
        {
            var req = new CreateUserRequest() { Name = "John" };
            
             _postHandler.Execute(Arg.Is<CreateUserCommand>(x=>x.Name == req.Name)).Returns(x=>new UserEntity(){ Name = "John2" });

            var actual = await Sut.Post(req);
            
            Assert.Equal("John2", actual.Name);
            
        }
    }
}