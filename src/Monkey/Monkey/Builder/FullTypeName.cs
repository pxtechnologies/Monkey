namespace Monkey.Builder
{
    public class FullTypeName
    {
        public string _namespace;
        public string _name;
        public string Name => _name;
        public string Namespace => _namespace;
        public bool IsNamespaceDefined => !string.IsNullOrWhiteSpace(Namespace);
        public FullTypeName(string fullType)
        {
            int ix = fullType.LastIndexOf(".");
            if (ix >= 0 && ix < fullType.Length-1)
            {
                _namespace = fullType.Remove(ix);
                _name = fullType.Substring(ix + 1);
            }
            else
            {
                _name = fullType;
            }
        }
    }
    public static class Extensions {
        public static FullTypeName ParseType(this string str)
        {
            return new FullTypeName(str);
        }
    }
}