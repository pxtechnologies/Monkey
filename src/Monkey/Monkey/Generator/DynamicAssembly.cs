using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Monkey.Compilation;

namespace Monkey.Generator
{
    public class FullTypeNameConflictException : Exception
    {
        public FullTypeNameConflictException(string conflictingName) : base($"Class full type name conflict: {conflictingName}")
        {
            ConflictingName = conflictingName;
        }

        public string ConflictingName { get; private set; }
    }
    public class SourceUnitCollection : IEnumerable<SourceUnit>
    {
        Dictionary<Guid, SourceUnit> _srcUnits = new Dictionary<Guid, SourceUnit>();
        Dictionary<string, SourceUnit> _nameUnits = new Dictionary<string, SourceUnit>();

        public bool Append(SourceUnit unit)
        {
            if (_srcUnits.TryGetValue(unit.CodeHash, out SourceUnit u))
            {
                if (u.FullName == unit.FullName)
                    return false;
                else throw new NotSupportedException("MD5 generation conflict.");
            }
            else
            {
                if (_nameUnits.TryGetValue(unit.FullName, out SourceUnit u2))
                    throw new FullTypeNameConflictException(u2.FullName);
                else
                {
                    _srcUnits.Add(unit.CodeHash, unit);
                    _nameUnits.Add(unit.FullName, unit);
                    return true;
                }
            }
        }

        public IEnumerator<SourceUnit> GetEnumerator()
        {
            foreach (var sourceUnit in _srcUnits)
            {
                yield return sourceUnit.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class DynamicAssembly
    {
        public Guid SrcHash {
            get { return string.Concat(_sourceUnits.Select(x => x.CodeHash.ToString())).ComputeMd5(); }
        }
        private SourceUnitCollection _sourceUnits;
        private Assembly _assembly;
        public DateTimeOffset CompilationDate { get; private set; }
        public TimeSpan CompilationDuration { get; private set; }
        private readonly List<Assembly> _references;

        public void AddReferenceFromType<TClass>()
        {
            var type = typeof(TClass);
            AddReferenceFromType(type);
        }

        public void AddReferenceFromType(Type type)
        {
            var assembly = type.Assembly;
            if (!_references.Contains(assembly))
                _references.Add(assembly);
        }

        public DynamicAssembly()
        {
            _sourceUnits = new SourceUnitCollection();
            _references = new List<Assembly>();
        }

        public Type Load(string ns, string typeName)
        {
            return _assembly.GetType($"{ns}.{typeName}");
        }

        public void SetSourceUnits(SourceUnitCollection collection)
        {
            _sourceUnits = collection;
        }
        public void AppendSourceUnits(IEnumerable<SourceUnit> sus)
        {
            foreach(var s in sus)
                _sourceUnits.Append(s);
        }

        public DynamicAssembly Compile(ITypeCompiler compiler)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            _assembly = compiler.FastLoad(string.Join(Environment.NewLine, 
                _sourceUnits.Select(x=>x.Code)), _references.ToArray());
            s.Stop();
            CompilationDate = DateTimeOffset.Now;
            CompilationDuration = s.Elapsed;
            return this;
        }
    }
}