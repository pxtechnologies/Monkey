using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monkey.Generator;

namespace Monkey.WebApi.Generator
{
    public class WebApiCqrsGenerator : ISourceCodeGenerator
    {
        private readonly IServiceMetadataProvider _cqrsMetadataRegister;

        public WebApiCqrsGenerator(IServiceMetadataProvider cqrsMetadataRegister)
        {
            _cqrsMetadataRegister = cqrsMetadataRegister;
        }

        public async Task<IEnumerable<SourceUnit>> Generate()
        {
            SourceUnitCollection collection = new SourceUnitCollection();

            foreach (var sourceUnit in _cqrsMetadataRegister.GetServices()
                .SelectMany(GenerateControllerForService))
            {
                collection.Append(sourceUnit);
            }

            return collection;
        }

        private SourceUnit[] GenerateControllerForService(ServiceInfo info)
        {
            List<SourceUnit> result = new List<SourceUnit>();

            

            return result.ToArray();
        }
    }
}