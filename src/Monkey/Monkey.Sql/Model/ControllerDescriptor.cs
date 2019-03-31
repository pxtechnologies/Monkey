using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Sql.Model
{
    public class ControllerDescriptor
    {
        public long Id { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        public string Route { get; set; }

        public List<ControllerActionDescriptor> Actions { get; set; }
    }
}