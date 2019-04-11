using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Monkey.Cqrs;
using Monkey.Logging;
using Monkey.SimpleInjector;
using Monkey.Sql.WebApiHost.Services;
using Monkey.WebApi;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Monkey.Sql.WebApiHost.Configuration
{
    public class HostPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables()
                .AddJsonFile("appsettings.json");

            var logger = LoggerConfig.Default().CreateLogger();
            container.RegisterInstance<Serilog.ILogger>(logger);
            MonkeyPackage.LoggerFactory = () => new SerilogAdapter(logger);

            var config = builder.Build();
            container.RegisterInstance<IConfigurationRoot>(config);
            container.RegisterInstance<IConfiguration>(config);
            container.Register<IServiceProvider>(() => container, Lifestyle.Singleton);
            
            container.Register(typeof(ICommandHandler<,>), typeof(DispatchingCommandHandler<,>), Lifestyle.Singleton);
        }
    }
}
