using System.ComponentModel.DataAnnotations;

namespace Monkey.Sql.Model
{
    public class ControllerActionDescriptor
    {
        public long Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Route { get; set; }

        public HttpVerb Verb { get; set; }

        public long ResponseId { get; set; }
        public ControllerResponse ControllerResponse { get; set; }
        public bool IsResponseCollection { get; set; }
    }
}