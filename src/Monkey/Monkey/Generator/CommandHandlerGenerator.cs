using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Monkey.Generator
{
    public interface ISourceCodeGenerator
    {
        Task<IEnumerable<SourceUnit>> Generate();
    }

    public interface IServiceMetadataProvider
    {
        IEnumerable<ServiceInfo> GetServices();
        
    }

    public class ServiceMetadataRegister : IServiceMetadataProvider
    {
        private readonly IServiceNameProvider _provider;
        private readonly IHandlerInfoFactory _handlerFactory;
        private readonly Dictionary<string,ServiceInfo> _services;

        public ServiceMetadataRegister(IServiceNameProvider provider, IHandlerInfoFactory handlerFactory)
        {
            _provider = provider;
            _handlerFactory = handlerFactory;
            _services = new Dictionary<string, ServiceInfo>();
        }

        public IEnumerable<ServiceInfo> GetServices() => _services.Values;

        public void Discover(params Assembly[] assemblies)
        {
            foreach (var handlerType in assemblies.SelectMany(x => x.GetTypes())
                .Where(x=>!x.IsAbstract && x.IsClass && !x.IsInterface))
            {
                var collection = handlerType.GetInterfaces()
                    .Where(x => x.IsGenericType && x.IsGenericTypeDefinition)
                    .Where(x => x.Namespace == "Monkey.Cqrs")
                    .Where(x => validaHandlers.Any(y => x.Name.StartsWith(y)))
                    .Select(interfaceType => _handlerFactory.Create(interfaceType, handlerType))
                    .Where(x => x != null);

                foreach (var handler in collection)
                {
                    string serviceName = _provider.EvaluateHandler(handler);
                    if (!_services.TryGetValue(serviceName, out ServiceInfo service))
                    {
                        service = new ServiceInfo(_handlerFactory);
                        service.WithName(serviceName);
                        _services.Add(serviceName, service);
                    }

                    service.AddHandler(handler);
                }

            }

        }
        private readonly string[] validaHandlers = new string[] { "IQueryHandler",
            "ISingleQueryHandler",
            "ICommandHandler"
        };
    }

    public class DynamicTypePool
    {
        private List<DynamicAssembly> _assemblies;

        public DynamicTypePool()
        {
            _assemblies = new List<DynamicAssembly>();
        }

    }
}
