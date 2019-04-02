using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Sql.Model
{
    public class ObjectProperty
    {
        public long Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }
        
        public long PropertyTypeId { get; set; }
        public virtual ObjectType PropertyType { get; set; }


        public bool IsCollection { get; set; }
        public long DeclaringTypeId { get; set; }
        public virtual ObjectType DeclaringType { get; set; }
        
    }
}