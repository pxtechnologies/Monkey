using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Monkey.Generator
{
    public interface IDynamicTypePool
    {
        Guid Signature { get; }
        IEnumerable<Assembly> GetAssemblies();
        DynamicTypePool Add(DynamicAssembly assembly);
    }

    public class DynamicTypePool : IDynamicTypePool
    {
        private LinkedList<DynamicAssembly> _assemblies;
        private Guid _signature;
        public Guid Signature { get; private set; }
        public DynamicTypePool()
        {
            _assemblies = new LinkedList<DynamicAssembly>();
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            //TODO: Change this
            if(_assemblies.Any())
                yield return _assemblies.Last.Value.Assembly;
        }
        public DynamicTypePool Add(DynamicAssembly assembly)
        {
            _signature = Guid.NewGuid();
            _assemblies.AddFirst(assembly);
            return this;

        }
    }
}