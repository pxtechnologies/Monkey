using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Monkey.Sql.AcceptanceTests.Configuration
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
        }
    }
}