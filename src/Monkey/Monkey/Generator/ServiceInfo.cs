using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Monkey.Generator
{
    public class ServiceInfo
    {
        private readonly IHandlerInfoFactory _factory;
        private readonly List<HandlerInfo> _handlers;
        public ServiceInfo(IHandlerInfoFactory factory)
        {
            _factory = factory;
            _handlers = new List<HandlerInfo>();
        }

        public ServiceInfo WithName(string name)
        {
            Name = name;
            return this;
        }

        public ServiceInfo AddHandler(Type interfaceType, Type handlerType)
        {
            var handler = _factory.Create(interfaceType, handlerType);
            if(handler == null)
                throw new ArgumentException("Cannot create handler with provided parameters.");
            return AddHandler(handler);
        }
        public ServiceInfo AddHandler<TInterface, THandler>()
        {
            return AddHandler(typeof(TInterface), typeof(THandler));
        }
        public ServiceInfo AddHandler(HandlerInfo handler)
        {
            if (Assembly == null) Assembly = handler.HandlerType.Assembly;
            if (Assembly != handler.HandlerType.Assembly)
                throw new ArgumentException("Handler is from different assembly.");

            var serviceName = handler.HandlerType.GetCustomAttribute<ServiceNameAttribute>();
            if (Name == null) Name = serviceName.ServiceName;
            if (serviceName!= null && serviceName.ServiceName != Name)
                throw new ArgumentException("This handler does not belong to this service");

            _handlers.Add(handler);
            handler.Service = this;
            return this;
        }

        public bool IsValid => !string.IsNullOrWhiteSpace(Name) && Assembly != null && Handlers.Any();
        public string Name { get; private set; }
        public Assembly Assembly { get; private set; }
        public IEnumerable<HandlerInfo> Handlers => _handlers;
    }
}