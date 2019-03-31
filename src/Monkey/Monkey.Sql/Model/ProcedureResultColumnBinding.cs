namespace Monkey.Sql.Model
{
    public class ProcedureResultColumnBinding
    {
        public ProcedureResultColumnBinding()
        {

        }
        public long ResultColumnColumnId { get; set; }
        public long PropertyId { get; set; }
        public ProcedureResultColumn ResultColumnColumn { get; set; }
        public ObjectProperty ObjectProperty { get; set; }
    }
}