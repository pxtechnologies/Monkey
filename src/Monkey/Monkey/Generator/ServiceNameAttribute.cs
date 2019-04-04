using System;

namespace Monkey.Generator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceNameAttribute : Attribute
    {
        private string _serviceName;
        public string ServiceName => _serviceName;

        public ServiceNameAttribute(string serviceName)
        {
            _serviceName = serviceName;
        }
    }
}