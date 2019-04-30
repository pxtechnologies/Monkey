using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Monkey.Generator;
using Monkey.PubSub;
using Monkey.WebApi.Generator;

namespace Monkey.WebApi
{
    public static class ConfigurationExtensions
    {
        public static string[] GetApplicationUrls(this IConfiguration config)
        {
            return config["ASPNETCORE_URLS"].Split(';');
        }
    }
    public class CqrsControllerProvider : ICqrsControllerProvider
    {
        private readonly IDynamicTypePool _dynamicPool;
        private readonly IWebApiGenerator _webApiGenerator;
        private readonly IEventHub _eventHub;
        public CqrsControllerProvider(IDynamicTypePool dynamicPool, IWebApiGenerator webApiGenerator, IEventHub eventHub)
        {
            _dynamicPool = dynamicPool;
            _webApiGenerator = webApiGenerator;
            _eventHub = eventHub;
        }

        public Type[] GetControllerTypes(IEnumerable<ServiceInfo> services)
        {
            var result = _webApiGenerator.Generate(services);

            DynamicAssembly assembly = new DynamicAssembly(_eventHub);
            
            assembly.AppendSourceUnits(result);
            assembly.AddWebApiReferences();

            AssemblyPurpose p = AssemblyPurpose.Handlers | AssemblyPurpose.Commands | AssemblyPurpose.Queries |
                                AssemblyPurpose.Results;
            assembly.AddReferenceFrom(_dynamicPool.Where(x=>(x.Purpose & p) > 0).Select(x=>x.Assembly));

            _dynamicPool.AddOrReplace(assembly);

            assembly.Compile();


            var controllerTypes = assembly.Assembly.GetTypes()
                .Where(x => typeof(ControllerBase).IsAssignableFrom(x))
                .ToArray();

            return controllerTypes;
        }
    }
}