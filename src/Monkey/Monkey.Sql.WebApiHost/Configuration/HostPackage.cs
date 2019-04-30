using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public static class ConfigurationFactory {
        public static IConfigurationRoot Load()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddCommandLine(Environment.GetCommandLineArgs());
                
            var config = builder.Build();
            return config;
        }
    }
    public class HostPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            var logger = LoggerConfig.Default().CreateLogger();
            container.RegisterInstance<Serilog.ILogger>(logger);
            MonkeyPackage.LoggerFactory = () => new SerilogAdapter(logger);

            var config = ConfigurationFactory.Load();
            container.RegisterInstance<IConfigurationRoot>(config);
            container.RegisterInstance<IConfiguration>(config);
            container.Register<IServiceProvider>(() => container, Lifestyle.Singleton);
            
            container.Register(typeof(ICommandHandler<,>), typeof(DispatchingCommandHandler<,>), Lifestyle.Singleton);
            container.Register(typeof(IQueryHandler<,>), typeof(DispatchingQueryHandler<,>), Lifestyle.Singleton);
        }
    }
}
