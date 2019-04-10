using System;
using System.Threading.Tasks;
using FluentAssertions;
using Monkey.Cqrs;
using NSubstitute;
using SimpleInjector;
using Xunit;

namespace Monkey.UnitTests
{
    public class NSubstituteSimpleinjectorIntegrationTests
    {
        [Fact]
        public async Task CanResolveMocked()
        {
            var mockedInstance = NSubstitute.Substitute.For(new Type[] {typeof(ICommandHandler<string, string>)}, null);
            Container c = new Container();
            c.Options.DefaultLifestyle = Lifestyle.Singleton;
            c.Register(typeof(ICommandHandler<string, string>), () => mockedInstance);
            c.Verify();

            c.GetInstance<ICommandHandler<string,string>>().Execute(Arg.Any<string>()).Returns("John");

            var result = await c.GetInstance<ICommandHandler<string, string>>().Execute("Elo");

            result.Should().Be("John");
        }
    }
}