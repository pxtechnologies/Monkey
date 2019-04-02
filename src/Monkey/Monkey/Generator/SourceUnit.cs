using System;


namespace Monkey.Generator
{
    public class SourceUnit
    {
        private readonly Lazy<Guid> _codeMd5;
        public SourceUnit(string ns, string typeName, string code)
        {
            Namespace = ns;
            TypeName = typeName;
            Code = code;
            _codeMd5 = new Lazy<Guid>(() => Code.ComputeMd5());
        }

        public Guid CodeHash => _codeMd5.Value;
        public string Code { get; private set; }
        public string TypeName { get; private set; }
        public string Namespace { get; private set; }
    }
}