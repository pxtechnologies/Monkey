using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Monkey.Services;

namespace Monkey.Generator
{
    public interface ISourceCodeGenerator
    {
        Task<IEnumerable<SourceUnit>> Generate();
    }

    public interface IServiceMetadataProvider
    {
        IEnumerable<ServiceInfo> GetServices();
        void Discover(params Assembly[] assemblies);
        void Discover(Predicate<HandlerInfo> filter, params Assembly[] assemblies);
    }

    public class ServiceMetadataRegister : IServiceMetadataProvider
    {
        private readonly IServiceNameProvider _provider;
        private readonly IHandlerInfoFactory _handlerFactory;
        private readonly IEnumerable<IMetadataChangedSubscriber> _subscribers;
        private readonly Dictionary<string,ServiceInfo> _services;

        public ServiceMetadataRegister(IServiceNameProvider provider, 
            IHandlerInfoFactory handlerFactory, 
            IEnumerable<IMetadataChangedSubscriber> subscribers)
        {
            _provider = provider;
            _handlerFactory = handlerFactory;
            _subscribers = subscribers;
            _services = new Dictionary<string, ServiceInfo>();
        }

        public IEnumerable<ServiceInfo> GetServices() => _services.Values;

        public void Discover(params Assembly[] assemblies)
        {
            Discover(null, assemblies);
        }
        public void Discover(Predicate<HandlerInfo> filter, params Assembly[] assemblies)
        {
            if (filter == null)
                filter = x => true;

            foreach (var handlerType in assemblies.SelectMany(x => x.GetTypes())
                .Where(x=>!x.IsAbstract && x.IsClass && !x.IsInterface))
            {
                var collection = handlerType.GetInterfaces()
                    .Where(x => x.IsGenericType)
                    .Where(x => x.Namespace == "Monkey.Cqrs")
                    .Where(x => validaHandlers.Any(y => x.Name.StartsWith(y)))
                    .Select(interfaceType => _handlerFactory.Create(interfaceType, handlerType))
                    .Where(x => x != null)
                    .Where(x => filter(x));

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
            foreach(var i in _subscribers)
                i.Changed();
        }
        private readonly string[] validaHandlers = new string[] { "IQueryHandler",
            "ISingleQueryHandler",
            "ICommandHandler"
        };
    }
}
