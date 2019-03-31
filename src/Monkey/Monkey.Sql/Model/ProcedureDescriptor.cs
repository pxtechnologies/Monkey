using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Monkey.Sql.Model
{
    public class ProcedureDescriptor
    {
        public long Id { get; set; }

        [Required, MaxLength(255)]
        public string ConnectionName { get; set; }

        [Required, MaxLength(255)]
        public string Schema { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }
    }
}
