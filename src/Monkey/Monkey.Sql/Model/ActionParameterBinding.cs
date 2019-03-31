using System.ComponentModel.DataAnnotations;

namespace Monkey.Sql.Model
{
    public class ActionParameterBinding
    {
        public ControllerActionDescriptor Action { get; set; }
        public long ActionId { get; set; }
        public ControllerRequest ControllerRequest { get; set; }
        public long RequestId { get; set; }

        public int Order { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }
        
        public bool IsFromUrl { get; set; }
        public bool IsFromBody { get; set; }

        
    }
}