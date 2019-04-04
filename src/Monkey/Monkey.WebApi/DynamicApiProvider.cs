using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Monkey.Generator;
using Monkey.WebApi.Generator;

namespace Monkey.WebApi
{
    public class DynamicApiProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private IServiceMetadataProvider _cqrsMetadataRegister;
        private ICqrsControllerProvider _cqrsControllerProvider;

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, 
            ControllerFeature feature)
        {
            var services = _cqrsMetadataRegister.GetServices();
            var controllers = _cqrsControllerProvider.GetControllerTypes(services);
            
            foreach(var t in controllers)
                feature.Controllers.Add(t.GetTypeInfo());
        }
    }
}