using System;
using System.Linq;
using Monkey.Compilation;
using Monkey.Cqrs;
using Monkey.Generator;
using Monkey.Logging;
using Monkey.Patterns.UnitTests;
using Monkey.Patterns.UnitTests.Handler;
using Monkey.Patterns.UnitTests.WebApi;
using Monkey.WebApi.Generator;
using Xunit;


namespace Monkey.WebApi.UnitTests
{
    public class CqrsControllerGeneratorTests 
    {
        Fixture Fx = new Fixture();
        class Fixture : TestFor<CqrsControllerGenerator>
        {
            private ILogger _logger;
            protected override CqrsControllerGenerator CreateSut()
            {
                return new CqrsControllerGenerator(_logger);
            }
        }
        private ServiceInfo Service()
        {
            return new ServiceInfo(new HandlerInfoFactory());
        }

        
        [Fact]
        public void CreateActionParses()
        {
            var service = Service().WithName("User")
                .AddHandler(typeof(ICommandHandler<CreateUserCommand, UserEntity>), typeof(CreateUserHandler));

            var srcUnits = this.Fx.Sut.Generate(service).ToArray();

            CSharpCodeAssertions.CodeParses(string.Join(Environment.NewLine, srcUnits.Select(x=>x.Code)));
        }

        [Fact]
        public void UpdateAction()
        {
            var service = Service().WithName("User")
                .AddHandler(typeof(ICommandHandler<UpdateUserCommand, UserEntity>), typeof(UpdateUserHandler));

            var srcUnits = this.Fx.Sut.Generate(service).ToArray();

            CSharpCodeAssertions.CodeParses(string.Join(Environment.NewLine, srcUnits.Select(x => x.Code)));
        }

        [Fact]
        public void CustomAction()
        {
            var service = Service().WithName("User")
                .AddHandler(typeof(ICommandHandler<ActivateUserCommand, UserEntity>), typeof(ActivateUserHandler));

            var srcUnits = this.Fx.Sut.Generate(service).ToArray();

            CSharpCodeAssertions.CodeParses(string.Join(Environment.NewLine, srcUnits.Select(x => x.Code)));
        }

        
    }
}