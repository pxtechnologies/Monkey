namespace Monkey.Sql.Model
{
    public class ProcedureParameterBinding
    {
        public ProcedureParameterBinding()
        {
            
        }
        public long ParameterId { get; set; }
        public long PropertyId { get; set; }
        public ProcedureParameterDescriptor Parameter { get; set; }
        public ObjectProperty ObjectProperty { get; set; }
    }
}