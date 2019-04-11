using System;
using Monkey.Compilation;
using Monkey.Extensions;
using Monkey.Generator;
using Monkey.Logging;
using Monkey.Services;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SimpleInjector.Packaging;

namespace Monkey.SimpleInjector
{
    public class MonkeyPackage : IPackage
    {
        static MonkeyPackage()
        {
            LoggerFactory = () => new ConsoleLogger();
        }
        public static Func<ILogger> LoggerFactory { get; set; }
        public void RegisterServices(Container container)
        {
            ServiceProviderExtension.ScopeFactory = s => AsyncScopedLifestyle.BeginScope(container);
            
            container.Register<ITypeCompiler, TypeCompiler>();
            container.Register<IServiceMetadataProvider, ServiceMetadataRegister>(Lifestyle.Singleton);
            container.Register<IServiceNameProvider, ServiceNameProvider>(Lifestyle.Singleton);
            container.Register<IHandlerInfoFactory, HandlerInfoFactory>(Lifestyle.Singleton);
            container.Register<ILogger>(LoggerFactory, Lifestyle.Singleton);
        }
    }
}
