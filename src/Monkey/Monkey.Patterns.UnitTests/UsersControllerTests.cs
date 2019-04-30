using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Monkey.Patterns.UnitTests.WebApi;
using NSubstitute;
using Xunit;

namespace Monkey.Patterns.UnitTests
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserRequest, CreateUserCommand>()
                .ForMember(x => x.Age, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());
                //.IgnoreAllUnmapped();

            CreateMap<Guid, CreateUserCommand>()
                .ForMember(x => x.Id, opt => opt.MapFrom(dst => dst))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UpdateUserRequest, UpdateUserCommand>()
                .IgnoreAllUnmapped();

            CreateMap<Guid, GetUserByIdQuery>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src))
                .IgnoreAllUnmapped();

            CreateMap<string, GetUserByNameQuery>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src))
                .IgnoreAllUnmapped();

            CreateMap<GetActiveUsersFromRequest, GetActiveUsersFromQuery>()
                .IgnoreAllUnmapped();

            CreateMap<UserEntity, UserResource>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name));
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
            config.AssertConfigurationIsValid();
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