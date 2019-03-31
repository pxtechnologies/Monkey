using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Sql.Model
{
    public class ObjectType
    {
        public long Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Namespace { get; set; }

        public bool IsPrimitive { get; set; }
        public bool IsVoid { get; set; }
        public bool IsDynamic { get; set; }
        

        public List<ObjectProperty> Properties { get; set; }

        public ObjectType()
        {
            Properties = new List<ObjectProperty>();
        }
    }
}