using System;
using System.Threading.Tasks;
using AutoMapper;
using Monkey.Patterns.UnitTests.WebApi;
using NSubstitute;
using Xunit;

namespace Monkey.Patterns.UnitTests
{
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
                cfg.CreateMap<CreateUserRequest, CreateUserCommand>();
                cfg.CreateMap<UpdateUserRequest, UpdateUserCommand>();
                
                cfg.CreateMap<Guid, GetUserByIdQuery>().ForMember(x=>x.Id, opt => opt.MapFrom(src=>src));
                cfg.CreateMap<string, GetUserByNameQuery>().ForMember(dst => dst.Name, opt => opt.MapFrom(src => src));
                cfg.CreateMap<GetActiveUsersFromRequest, GetActiveUsersFromQuery>();
                cfg.CreateMap<UserEntity, UserResource>();
                
            });
            
            _mapper = new Mapper(config); 
        }

        protected override UsersController CreateSut()
        {
            return new UsersController(_postHandler, _putHandler, _getUsers, _getUserById, _getUserByName, _getActiveUsersFrom, _mapper);
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