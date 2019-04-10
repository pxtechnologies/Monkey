using SimpleInjector;

namespace Monkey.WebApi.AcceptanceTests.Configuration
{
    public class ContainerAccessor
    {
        public ContainerAccessor(Container container)
        {
            Container = container;
        }

        public Container Container { get; private set; }
    }
}