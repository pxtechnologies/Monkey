namespace Monkey.Sql.Model
{
    public class ProcedureBinding
    {
        public long ProcedureId { get; set; }
        public ProcedureDescriptor Procedure { get; set; }

        public long ResultId { get; set; }
        public Result Result { get; set; }
        public bool IsResultCollection { get; set; }
        public Mode Mode { get; set; }
    }
}