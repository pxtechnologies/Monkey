using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

using Monkey.Compilation;
using Monkey.PubSub;

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
        private string _code;
        public long Version { get; set; }
        public string Code
        {
            get
            {
                if (_code == null)
                    _code = string.Join(Environment.NewLine, this.Select(x => x.Code));
                return _code;
            }
            
        }

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
                    _code = null;
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
        public Guid SrcHash
        {
            get { return string.Concat(this.Select(x => x.CodeHash.ToString())).ComputeMd5(); }
        }
        public void Append(SourceUnitCollection collection)
        {
            foreach (var i in collection)
                this.Append(i);
        }
    }
    public class DynamicAssembly
    {
        public AssemblyPurpose Purpose { get; set; }
        public Guid SrcHash {
            get { return string.Concat(_sourceUnits.Select(x => x.CodeHash.ToString())).ComputeMd5(); }
        }
        private SourceUnitCollection _sourceUnits;
        private Assembly _assembly;
        public Assembly Assembly => _assembly;
        public DateTimeOffset CompilationDate { get; private set; }
        public TimeSpan CompilationDuration { get; private set; }
        private readonly List<Assembly> _references;
        public SourceUnitCollection SourceUnits => _sourceUnits;
        public IEnumerable<Assembly> References => _references;
        public void AddReferenceFromType<TClass>()
        {
            var type = typeof(TClass);
            AddReferenceFromType(type);
        }
        public void AddReferenceFromTypes(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                AddReferenceFromType(type);
            }
        }
        public void AddReferenceFrom(IEnumerable<Assembly> assemblies)
        {
            _references.AddRange(assemblies);
        }
        public void AddReferenceFromType(Type type)
        {
            var assembly = type.Assembly;
            if (!_references.Contains(assembly))
                _references.Add(assembly);
        }

        public DynamicAssembly(IEventHub eventHub)
        {
            _eventHub = eventHub;
            _sourceUnits = new SourceUnitCollection();
            _references = new List<Assembly>();
        }

        public Type Load(string ns, string typeName)
        {
            return _assembly.GetType($"{ns}.{typeName}");
        }
        public Type Load(string fullType)
        {
            return _assembly.GetType(fullType);
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

        public DynamicAssembly Compile(ITypeCompiler compiler = null)
        {
            if (compiler == null) compiler = new TypeCompiler();
            Stopwatch s = new Stopwatch();
            s.Start();
            _assembly = compiler.FastLoad(_sourceUnits.Code, _references.ToArray());
            s.Stop();
            CompilationDate = DateTimeOffset.Now;
            CompilationDuration = s.Elapsed;

            _eventHub.Publish(new AssemblyCompiledEvent()
            {
                Assembly = _assembly,
                Duration = CompilationDuration,
                SourceCode = _sourceUnits,
                When = CompilationDate,
                Purpose = this.Purpose,
                Data = File.ReadAllBytes(_assembly.Location)
            });
            return this;
        }

        private IEventHub _eventHub;
    }

    public class AssemblyCompiledEvent
    {
        public SourceUnitCollection SourceCode { get; set; }
        public Assembly Assembly { get; set; }
        public DateTimeOffset When { get; set; }
        public TimeSpan Duration { get; set; }
        public AssemblyPurpose Purpose { get; set; }
        public byte[] Data { get; set; }
        public string Errors { get; set; }
    }
    [Flags]
    public enum AssemblyPurpose
    {
        None = 0,
        Handlers = 1,
        Commands = 2,
        Queries = 4,
        Results = 8,
        Requests = 16,
        Responses = 32,
        RequestProfiles = 64,
        ResponseProfiles = 128,
        Controllers = 256
    }
}