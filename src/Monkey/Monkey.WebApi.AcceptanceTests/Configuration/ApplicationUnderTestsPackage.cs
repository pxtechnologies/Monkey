using Microsoft.Extensions.Configuration;
using Monkey.Cqrs;
using Monkey.WebApi.AcceptanceTests.Integration;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Monkey.WebApi.AcceptanceTests.Configuration
{
    public class ApplicationUnderTestsPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables()
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            container.RegisterInstance<IConfigurationRoot>(config);
            container.RegisterInstance<IConfiguration>(config);
            container.Register<MockRegister>(Lifestyle.Singleton);
            container.Register(typeof(ICommandHandler<,>), typeof(Integration.DispatchingCommandHandler<,>), Lifestyle.Singleton);
        }
    }
}