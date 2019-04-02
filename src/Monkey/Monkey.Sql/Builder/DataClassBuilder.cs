using System.Collections.Generic;
using System.Linq;

namespace Monkey.Sql.Builder
{
    
    public class DataClassBuilder : ICodeBlockBuilder
    {

        private List<string> _interfaces;
        private List<IField> _props;
        private string _name;
        private string _nameSpace;
        public DataClassBuilder()
        {
            _name = "Dto";
            _nameSpace = "";
            _props = new List<IField>();
            _interfaces = new List<string>();
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

        public string GenerateCode()
        {
            SourceCodeBuilder s=  new SourceCodeBuilder();
            GenerateCode(s);
            return s.ToString();
        }
        public void GenerateCode(SourceCodeBuilder sb)
        {
            
            if (!string.IsNullOrWhiteSpace(_nameSpace))
            {
                sb.AppendLine($"namespace {_nameSpace}");
                sb.AppendLine("{");
                sb.IndentUp();
            }

            sb.AppendLine($"public class {_name}");
            if (_interfaces.Any())
            {
                sb.Remove(sb.Length - 2, 2);
                sb.Append(" : ");
                sb.Append(string.Join(",", _interfaces));
                sb.AppendLine();
            }

            sb.AppendLine("{").IndentUp();


            foreach (var prop in _props)
            {
                sb.AppendLine($"public {prop.Type} {prop.Name} {{ get; set; }}");
            }
            
            sb.IndentDown().AppendLine("}");
            
            if (!string.IsNullOrWhiteSpace(_nameSpace))
            {
                sb.AppendLine("}");
                sb.IndentDown();
            }
        }
    }
    
}
