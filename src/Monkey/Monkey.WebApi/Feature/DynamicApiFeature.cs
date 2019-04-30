using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Monkey.Compilation;
using Monkey.Generator;
using Monkey.WebApi.Generator;

namespace Monkey.WebApi.Feature
{
    public class DynamicApiFeature
    {
        private readonly IServiceMetadataProvider _cqrsMetadataRegister;
        private readonly ICqrsControllerProvider _cqrsControllerProvider;
        private readonly List<TypeInfo> _staticControllers;
        private bool _isFirstRun = true;
        public void PopulateFeature(IEnumerable<ApplicationPart> parts,
            ControllerFeature feature)
        {
            if (_isFirstRun)
            {
                _staticControllers.AddRange(feature.Controllers.Where(x=>x.Assembly.GetCustomAttribute<MonkeyGeneratedAttribute>() == null));
                _isFirstRun = false;
            }
            var services = _cqrsMetadataRegister.GetServices()
                .ToArray();

            if (!services.Any())
                return;

            feature.Controllers.Clear();
            foreach (var c in _staticControllers)
                feature.Controllers.Add(c);

            var controllers = _cqrsControllerProvider.GetControllerTypes(services);

            foreach (var t in controllers)
                feature.Controllers.Add(t.GetTypeInfo());
        }
        public DynamicApiFeature(IServiceMetadataProvider cqrsMetadataRegister, ICqrsControllerProvider cqrsControllerProvider)
        {
            _cqrsMetadataRegister = cqrsMetadataRegister;
            _cqrsControllerProvider = cqrsControllerProvider;
            _staticControllers = new List<TypeInfo>();
        }
    }
}