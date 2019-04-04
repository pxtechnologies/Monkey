using System.Collections.Generic;
using System.Linq;

namespace Monkey.Builder
{
    
    public class DataClassBuilder : ICodeBlockBuilder
    {

        private List<string> _interfaces;
        private List<IField> _props;
        private List<string> _usingNs;
        private string _name;
        private string _nameSpace;
        public DataClassBuilder AddUsing(string ns)
        {
            if(!_usingNs.Contains(ns))
                _usingNs.Add(ns);
            return this;
        }

        public DataClassBuilder()
        {
            _name = "Dto";
            _nameSpace = "";
            _props = new List<IField>();
            _interfaces = new List<string>();
            _usingNs = new List<string>(){ "System" };
        }

        public DataClassBuilder WithName(string name)
        {
            this._name = name;
            return this;
        }

        public DataClassBuilder InNamespace(string ns)
        {
            this._nameSpace = ns;
            return this;
        }

        public DataClassBuilder WithProperties(params IField[] fields)
        {
            foreach (var f in fields)
                WithProperty(f);
            return this;
        }

        public DataClassBuilder ImplementsInterface(string type)
        {
            _interfaces.Add(type);
            return this;
        }
        public DataClassBuilder WithProperty(IField field)
        {
            _props.Add(field);
            return this;
        }
        public DataClassBuilder WithProperty(string type, string name)
        {
            WithProperty(new Field(type, name));
            return this;
        }

        
        public void GenerateCode(SourceCodeBuilder sb)
        {
            
            if (!string.IsNullOrWhiteSpace(_nameSpace))
            {
                sb.AppendLine($"namespace {_nameSpace}");
                sb.OpenBlock();
            }

            if (_usingNs.Any())
            {
                foreach (var n in _usingNs)
                    sb.AppendLine($"using {n};");
            }

            sb.AppendLine();

            sb.AppendLine($"public class {_name}");
            if (_interfaces.Any())
            {
                sb.Remove(sb.Length - 2, 2);
                sb.Append(" : ");
                sb.Append(string.Join(",", _interfaces));
                sb.AppendLine();
            }

            sb.OpenBlock();


            foreach (var prop in _props)
            {
                sb.AppendLine($"public {prop.Type} {prop.Name} {{ get; set; }}");
            }

            sb.CloseBlock();
            
            if (!string.IsNullOrWhiteSpace(_nameSpace))
            {
                sb.CloseBlock();
            }
        }
    }
    
}
