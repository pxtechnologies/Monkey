using Monkey.Builder;

namespace Monkey.Sql.Builder
{
    public class BindingResult
    {
        public string Path { get; set; }
        public FullTypeNameInfo PropertyType { get; set; }

        public bool IsPropertyNullable
        {
            get { return PropertyType.IsNullable; }
        }

        

        public string SqlColumnName { get; set; }
    }
}