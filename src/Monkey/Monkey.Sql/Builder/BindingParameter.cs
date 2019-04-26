using Monkey.Builder;

namespace Monkey.Sql.Builder
{
    public class BindingParameter
    {
        private string _parameterName;
        private string _path;
        

        public FullTypeNameInfo ParameterType { get; set; }
        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}