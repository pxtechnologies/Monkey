using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Monkey.PubSub;

namespace Monkey.Generator
{
    public interface IDynamicTypePool : IEnumerable<DynamicAssembly>
    {
        Guid Signature { get; }
        IEnumerable<Assembly> GetAssemblies();
        DynamicTypePool AddOrReplace(DynamicAssembly assembly);
        bool CanMerge { get; }
        DynamicAssembly Merge(IEnumerable<SourceUnit> units, AssemblyPurpose referencePurposes = AssemblyPurpose.None);
    }

    public class DynamicTypePool : IDynamicTypePool
    {
        private LinkedList<DynamicAssembly> _assemblies;
        private Guid _signature;
        public Guid Signature => _signature;
        public DynamicTypePool(IEventHub eventHub)
        {
            _eventHub = eventHub;
            _assemblies = new LinkedList<DynamicAssembly>();
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            //TODO: Change this
            foreach (var i in _assemblies)
                yield return i.Assembly;
        }
        public bool CanMerge
        {
            get { return _assemblies.Any(); }
        }

        private IEventHub _eventHub;
        public DynamicAssembly Merge(IEnumerable<SourceUnit> units, AssemblyPurpose referencePurposes = AssemblyPurpose.None)
        {
            //TODO: Rething this...

            var newOnes = units.Except(_assemblies.SelectMany(x => x.SourceUnits));
            if (newOnes.Any())
            {
                DynamicAssembly n = new DynamicAssembly(_eventHub);
                
                n.AddReferenceFrom(_assemblies.First().References);
                n.AppendSourceUnits(newOnes);
                if (referencePurposes != AssemblyPurpose.None)
                {
                    var refs = _assemblies.Where(x => (x.Purpose & referencePurposes) > 0)
                        .Select(x=>x.Assembly)
                        .ToArray();
                    n.AddReferenceFrom(refs);
                }
                this._assemblies.AddFirst(n);
                return n.Compile();
            }
            else return _assemblies.First();
        }
        public DynamicTypePool AddOrReplace(DynamicAssembly assembly)
        {
            //TODO: Should be based on class-full-name
            _signature = Guid.NewGuid();
            var toReplace = _assemblies.FirstOrDefault(x => x.Purpose == assembly.Purpose);
            if (toReplace != null)
                _assemblies.Remove(toReplace);
            _assemblies.AddFirst(assembly);
            return this;

        }

        public IEnumerator<DynamicAssembly> GetEnumerator()
        {
            return _assemblies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}