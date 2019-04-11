using System;
using Monkey.Compilation;
using Monkey.Generator;
using Monkey.Logging;
using SimpleInjector;
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
            container.Register<ITypeCompiler, TypeCompiler>();
            container.Register<IServiceMetadataProvider, ServiceMetadataRegister>(Lifestyle.Singleton);
            container.Register<IServiceNameProvider, ServiceNameProvider>(Lifestyle.Singleton);
            container.Register<IHandlerInfoFactory, HandlerInfoFactory>(Lifestyle.Singleton);
            container.Register<ILogger>(LoggerFactory, Lifestyle.Singleton);
        }
    }
}
