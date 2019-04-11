using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monkey.Generator;

namespace Monkey.WebApi.Generator
{
    public interface IWebApiGenerator
    {
        SourceUnitCollection Generate(IEnumerable<ServiceInfo> serviceInfos);
    }
    public class WebApiCqrsGenerator : IWebApiGenerator
    {
        private readonly ICqrsControllerGenerator _cqrsControllerGenerator;
        public WebApiCqrsGenerator(ICqrsControllerGenerator cqrsControllerGenerator)
        {
            _cqrsControllerGenerator = cqrsControllerGenerator;
        }
        

        public SourceUnitCollection Generate(IEnumerable<ServiceInfo> serviceInfos)
        {
            SourceUnitCollection collection = new SourceUnitCollection();
            foreach (var sourceUnit in serviceInfos.SelectMany(_cqrsControllerGenerator.Generate))
            {
                collection.Append(sourceUnit);
            }

            return collection;
        }
    }
}