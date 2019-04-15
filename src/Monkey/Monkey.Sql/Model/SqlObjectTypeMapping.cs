using System.ComponentModel.DataAnnotations;

namespace Monkey.Sql.Model
{
    public class SqlObjectTypeMapping
    {
        public long Id { get; set; }
        public long ObjectTypeId { get; set; }
        public virtual PrimitiveObject ObjectType { get; set; }
        [MaxLength(255)]
        public string SqlType { get; set; }
        public bool IsNullable { get; set; }
    }
}