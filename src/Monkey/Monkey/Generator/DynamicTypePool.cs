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
        bool CanMerge { get; }
        DynamicAssembly Merge(IEnumerable<SourceUnit> units);
    }

    public class DynamicTypePool : IDynamicTypePool
    {
        private LinkedList<DynamicAssembly> _assemblies;
        private Guid _signature;
        public Guid Signature => _signature;
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
        public bool CanMerge
        {
            get { return _assemblies.Any(); }
        }

        public DynamicAssembly Merge(IEnumerable<SourceUnit> units)
        {
            //TODO: Rething this...

            var newOnes = units.Except(_assemblies.SelectMany(x => x.SourceUnits));
            if (newOnes.Any())
            {
                DynamicAssembly n = new DynamicAssembly();
                this._assemblies.AddFirst(n);
                n.AddReferenceFrom(_assemblies.First().References);
                n.AppendSourceUnits(newOnes);
                return n.Compile();
            }
            else return _assemblies.First();
        }
        public DynamicTypePool Add(DynamicAssembly assembly)
        {
            _signature = Guid.NewGuid();
            _assemblies.AddFirst(assembly);
            return this;

        }
    }
}