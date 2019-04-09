using System;
using Monkey.Compilation;
using Monkey.Logging;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Monkey.SimpleInjector
{
    public class MonkeyPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<ITypeCompiler, TypeCompiler>();
            container.Register<ILogger, ConsoleLogger>();
        }
    }
}
