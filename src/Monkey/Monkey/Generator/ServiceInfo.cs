﻿using System;
using System.Collections.Generic;
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
            return this;
        }
        public string Name { get; private set; }
        public Assembly Assembly { get; private set; }
        public IEnumerable<HandlerInfo> Handlers => _handlers;
    }
}