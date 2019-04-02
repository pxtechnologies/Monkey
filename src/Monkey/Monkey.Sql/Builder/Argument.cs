using System.Text;

namespace Monkey.Sql.Builder
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

        public new Argument WithAttribute(string at)
        {
            base.WithAttribute(at);
            return this;
        }
        public void Write(StringBuilder sb)
        {
            WriteAttributes(sb);
            sb.Append($"{_type} {_name}");
        }
    }
}