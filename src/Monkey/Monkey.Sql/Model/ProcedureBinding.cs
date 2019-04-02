using System.ComponentModel.DataAnnotations;

namespace Monkey.Sql.Model
{
    public class ProcedureBinding
    {
        public long ProcedureId { get; set; }
        public virtual ProcedureDescriptor Procedure { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        public long RequestId { get; set; }
        public virtual ObjectType Request { get; set; }

        public long ResultId { get; set; }
        public virtual Result Result { get; set; }
        public bool IsResultCollection { get; set; }
        public Mode Mode { get; set; }
    }
}