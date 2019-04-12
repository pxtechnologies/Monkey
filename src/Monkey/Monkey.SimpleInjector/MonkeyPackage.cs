using System;
using System.Linq;
using System.Reflection;
using Monkey.Compilation;
using Monkey.Extensions;
using Monkey.Generator;
using Monkey.Logging;
using Monkey.PubSub;
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
            
            container.Register<IEventHub, EventHub>(Lifestyle.Singleton);
            container.Register<ITypeCompiler, TypeCompiler>();
            container.Register<IServiceMetadataProvider, ServiceMetadataRegister>(Lifestyle.Singleton);
            container.Register<IServiceNameProvider, ServiceNameProvider>(Lifestyle.Singleton);
            container.Register<IHandlerInfoFactory, HandlerInfoFactory>(Lifestyle.Singleton);
            container.Register<ILogger>(LoggerFactory, Lifestyle.Singleton);
            container.Register<IDynamicTypePool, DynamicTypePool>(Lifestyle.Singleton);
        }
    }
}
