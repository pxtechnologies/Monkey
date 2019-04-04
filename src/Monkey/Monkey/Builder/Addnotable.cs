using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monkey.Builder
{
    public class Addnotable
    {
        private List<string> _attributes;

        public Addnotable()
        {
            _attributes = new List<string>();
        }

        public virtual Addnotable WithAttribute(string at)
        {
            _attributes.Add(at);
            return this;
        }
        public void WriteAttributes(StringBuilder sb)
        {
            if (_attributes.Any())
            {
                var atts = string.Join(",", _attributes);
                sb.AppendFormat($"[{atts}] ");
            }
        }
        public void WriteAttributes(SourceCodeBuilder sb)
        {
            if(_attributes.Any())
            {
                var atts = string.Join(",", _attributes);
                sb.AppendFormat($"[{atts}] ");
            }
        }
    }
}