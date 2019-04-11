using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Monkey.Generator;
using Monkey.WebApi.Generator;

namespace Monkey.WebApi
{
    public class CqrsControllerProvider : ICqrsControllerProvider
    {
        private readonly IDynamicTypePool _dynamicPool;
        private readonly IWebApiGenerator _webApiGenerator;
        public CqrsControllerProvider(IDynamicTypePool dynamicPool, IWebApiGenerator webApiGenerator)
        {
            _dynamicPool = dynamicPool;
            _webApiGenerator = webApiGenerator;
        }

        public Type[] GetControllerTypes(IEnumerable<ServiceInfo> services)
        {
            var result = _webApiGenerator.Generate(services);

            DynamicAssembly assembly = null;
            if (_dynamicPool.CanMerge)
                assembly = _dynamicPool.Merge(result);
            else
            {
                assembly = new DynamicAssembly();
                _dynamicPool.Add(assembly);
                assembly.AppendSourceUnits(result);
                assembly.AddWebApiReferences();
                assembly.Compile();
            }

            return assembly.Assembly.GetTypes()
                .Where(x => typeof(ControllerBase).IsAssignableFrom(x))
                .ToArray();
        }
    }
}