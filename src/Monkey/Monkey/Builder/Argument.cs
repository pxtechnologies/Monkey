using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monkey.Builder
{
    public class Argument : Addnotable
    {
        private string _name;
        private string _type;

        public Argument(string type,string name )
        {
            _name = name;
            _type = type;
        }

        public string Type => _type;
        public string Name => _name;

        public string ToDeclaration()
        {
            StringBuilder sb = new StringBuilder();
            base.WriteAttributes(sb);
            sb.AppendFormat($"{_type} {_name}");
            return sb.ToString();
        }

        public new Argument WithAttribute(string at)
        {
            base.WithAttribute(at);
            return this;
        }
        public void Write(SourceCodeBuilder sb)
        {
            WriteAttributes(sb);
            sb.Append($"{_type} {_name}");
        }
    }

    public class ArgumentCollection : IEnumerable<Argument>
    {
        private readonly List<Argument> _list;

        public ArgumentCollection()
        {
            _list =new List<Argument>();
        }
        public ArgumentCollection(IEnumerable<Argument> requestArguments)
        {
            _list = new List<Argument>(requestArguments);
        }
        public Argument Add(string type, string name, params string[] attributes)
        {
            Argument result = new Argument(type, name);
            foreach (var attribute in attributes)
            {
                result.WithAttribute(attribute);
            }
            _list.Add(result);
            return result;
        }

        public override string ToString()
        {

            return string.Join(", ", this.Select(x => x.ToDeclaration()));
        }

        public Argument Add(string type, string name)
        {
            Argument result = new Argument(type,name);
            _list.Add(result);
            return result;
        }

        public IEnumerator<Argument> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}