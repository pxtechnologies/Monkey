using System;
using System.Collections.Generic;
using System.Text;

namespace Monkey.Generator
{
    public interface ISourceCodeGenerator
    {
        IEnumerable<SourceUnit> Generate();
    }
    

    public interface ICqrsMetadataProvider
    {
        IEnumerable<Type> GetCommandHandlers();
        IEnumerable<Type> GetQueryHandlers();
    }
    public class DynamicTypePool
    {
        private List<DynamicAssembly> _assemblies;

        public DynamicTypePool()
        {
            _assemblies = new List<DynamicAssembly>();
        }

    }
}
