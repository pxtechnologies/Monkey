using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Basic;
using dbo;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Monkey.Patterns.UnitTests.Handler
{
    public class SqlTemplateTests
    {
        [Fact]
        public async Task Excute()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables()
                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            AddUserCommandHandler handler = new AddUserCommandHandler(config);

            var cmd = new AddUserCommand()
            {
                BirthDate = DateTime.Now,
                Id = 123,
                Name = "John"
            };
            var result = await handler.Execute(cmd);

            Assert.Equal("John!", result.Name);
            Assert.Equal(124, result.Id);
            Assert.Equal(cmd.BirthDate, cmd.BirthDate);
        }
    }
}
