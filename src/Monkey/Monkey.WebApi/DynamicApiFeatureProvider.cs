using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Monkey.WebApi
{
    public class DynamicApiFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IServiceProvider _container;
        private readonly Func<bool> _isReady;

        public DynamicApiFeatureProvider(IServiceProvider container, Func<bool> isReady)
        {
            _container = container;
            _isReady = isReady;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts,
            ControllerFeature feature)
        {
            if (_isReady())
            {
                var f = (DynamicApiFeature) _container.GetService(typeof(DynamicApiFeature));
                f.PopulateFeature(parts, feature);
            }
        }
    }
}