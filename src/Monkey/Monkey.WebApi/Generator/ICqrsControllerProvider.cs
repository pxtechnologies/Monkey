using System;
using System.Collections.Generic;
using Monkey.Generator;

namespace Monkey.WebApi.Generator
{
    public interface ICqrsControllerProvider
    {
        Type[] GetControllerTypes(IEnumerable<ServiceInfo> services);
    }
}