using System;
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

        [MaxLength(32)]
        public string Alias { get; set; }

        public bool IsPrimitive { get; set; }
        public bool IsVoid { get; set; }
        public bool IsDynamic { get; set; }

        public string SrcName()
        {
            if (IsPrimitive) return Alias;
            return FullName();
        }

        public string FullName()
        {
            if (String.IsNullOrWhiteSpace(Namespace))
                return Name;
            else return $"{Namespace}.{Name}";
        }
        public virtual List<ObjectProperty> Properties { get; set; }

        public ObjectType()
        {
            Properties = new List<ObjectProperty>();
        }
    }
}