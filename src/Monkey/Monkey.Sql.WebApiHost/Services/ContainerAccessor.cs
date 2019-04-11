using SimpleInjector;

namespace Monkey.Sql.WebApiHost.Services
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