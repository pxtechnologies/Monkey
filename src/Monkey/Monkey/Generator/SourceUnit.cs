using System;


namespace Monkey.Generator
{
    public class SourceUnit
    {
        private readonly Lazy<Guid> _codeMd5;
        public SourceUnit(string ns, string typeName, string code, long version = 0)
        {
            Namespace = ns;
            TypeName = typeName;
            Code = code;
            _codeMd5 = new Lazy<Guid>(() => Code.ComputeMd5());
            Version = version;
        }

        protected bool Equals(SourceUnit other)
        {
            return Equals(CodeHash, other.CodeHash);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SourceUnit) obj);
        }

        public override int GetHashCode()
        {
            return CodeHash.GetHashCode();
        }

        public long Version { get; private set; }
        public Guid CodeHash => _codeMd5.Value;
        public string Code { get; private set; }
        public string TypeName { get; private set; }
        public string Namespace { get; private set; }
        public string FullName
        {
            get { return $"{Namespace}.{TypeName}"; }
        }
    }
}