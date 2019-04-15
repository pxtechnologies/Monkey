namespace Monkey.Sql.Model
{
    public class ProcedureResultColumnBinding
    {
        public ProcedureResultColumnBinding()
        {

        }
        public virtual long ResultColumnId { get; set; }
        public virtual long PropertyId { get; set; }
        public virtual ProcedureResultColumn ResultColumn { get; set; }
        public virtual ObjectProperty Property { get; set; }
    }
}