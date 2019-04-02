namespace Monkey.Sql.Model
{
    public class ProcedureParameterBinding
    {
        public ProcedureParameterBinding()
        {
            
        }
        public long ParameterId { get; set; }
        public long PropertyId { get; set; }
        public virtual ProcedureParameterDescriptor Parameter { get; set; }
        public virtual ObjectProperty ObjectProperty { get; set; }
    }
}