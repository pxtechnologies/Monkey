namespace Monkey.Sql.Builder
{
    public class BindingResult
    {
        public string Path { get; set; }
        public string PropertyType { get; set; }

        public bool IsPropertyNullable
        {
            get { return PropertyType.EndsWith("?"); }
        }

        

        public string SqlColumnName { get; set; }
    }
}