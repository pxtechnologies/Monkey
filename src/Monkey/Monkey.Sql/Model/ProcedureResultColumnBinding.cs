namespace Monkey.Sql.Model
{
    public class ProcedureResultColumnBinding
    {
        public ProcedureResultColumnBinding()
        {

        }
        public long ResultColumnColumnId { get; set; }
        public long ObjectPropertyId { get; set; }
        public virtual ProcedureResultColumn ResultColumnColumn { get; set; }
        public virtual ObjectProperty ObjectProperty { get; set; }
    }
}