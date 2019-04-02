using System;
using Monkey.Compilation;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Monkey.SimpleInjector
{
    public class MonkeyPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<ITypeCompiler, TypeCompiler>();
        }
    }
}
