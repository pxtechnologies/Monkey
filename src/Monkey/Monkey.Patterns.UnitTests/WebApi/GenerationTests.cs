using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using dbo2;
using dbo2.WebApi;
using dbo2.WebApi.Profiles;
using mrxylpod.WebApi;
using Monkey.Cqrs;
using Xunit;

namespace Monkey.Patterns.UnitTests.WebApi
{
    public class GenerationTests
    {
        [Fact]
        public async Task Auto()
        {
            AutoMapper.Mapper m = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AddUserCommandAddUserResultResponseProfile>();
                cfg.AddProfile<AddUserRequestAddUserCommandProfile>();
            }));
            ICommandHandler<AddUserCommand, AddUserResult> h = NSubstitute.Substitute
                .For<ICommandHandler<AddUserCommand, AddUserResult>>();

            UserController c = new UserController(h, m);

            await c.Add(123, new AddUserRequest() {Birthdate = DateTime.Now, Name = "Hello"});
        }
    }
}
