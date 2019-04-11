using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Monkey.Generator;
using Monkey.WebApi.Generator;

namespace Monkey.WebApi.Feature
{
    public class DynamicApiFeature
    {
        private readonly IServiceMetadataProvider _cqrsMetadataRegister;
        private readonly ICqrsControllerProvider _cqrsControllerProvider;
        public void PopulateFeature(IEnumerable<ApplicationPart> parts,
            ControllerFeature feature)
        {
            var services = _cqrsMetadataRegister.GetServices()
                .ToArray();

            if (!services.Any())
                return;

            var controllers = _cqrsControllerProvider.GetControllerTypes(services);

            foreach (var t in controllers)
                feature.Controllers.Add(t.GetTypeInfo());
        }
        public DynamicApiFeature(IServiceMetadataProvider cqrsMetadataRegister, ICqrsControllerProvider cqrsControllerProvider)
        {
            _cqrsMetadataRegister = cqrsMetadataRegister;
            _cqrsControllerProvider = cqrsControllerProvider;
        }
    }
}