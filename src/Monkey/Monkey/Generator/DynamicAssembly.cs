using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Monkey.Compilation;

namespace Monkey.Generator
{
    public class DynamicAssembly
    {
        public Guid SrcHash {
            get { return string.Concat(_sourceUnits.Select(x => x.CodeHash.ToString())).ComputeMd5(); }
        }
        private readonly List<SourceUnit> _sourceUnits;
        private Assembly _assembly;
        public DateTimeOffset CompilationDate { get; private set; }
        public TimeSpan CompilationDuration { get; private set; }
        
        public DynamicAssembly()
        {
            _sourceUnits = new List<SourceUnit>();
        }

        public Type Load(string ns, string typeName)
        {
            return _assembly.GetType($"{ns}.{typeName}");
        }

        public bool AppendSource(SourceUnit add)
        {
            if(_sourceUnits.Any(x=>x.TypeName == add.TypeName && x.Namespace == add.Namespace))
                return false;
            if (_sourceUnits.Any(x => x.CodeHash == add.CodeHash))
                return false;
            _sourceUnits.Add(add);
            return true;
        }

        public void Compile(ITypeCompiler compiler)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            _assembly = compiler.FastLoad(string.Join(Environment.NewLine, _sourceUnits));
            s.Stop();
            CompilationDate = DateTimeOffset.Now;
            CompilationDuration = s.Elapsed;
        }
    }
}