using System.ComponentModel.DataAnnotations;

namespace Monkey.Sql.Model
{
    public class ProcedureParameterDescriptor
    {
        public long Id { get; set; }
        public byte Order { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Type { get; set; }

        public long ProcedureId { get; set; }
        public virtual ProcedureDescriptor Procedure { get; set; }
    }

    public class ProcedureResultColumn
    {
        public long Id { get; set; }
        public byte Order { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Type { get; set; }

        public long ProcedureId { get; set; }
        public virtual ProcedureDescriptor Procedure { get; set; }
    }
}