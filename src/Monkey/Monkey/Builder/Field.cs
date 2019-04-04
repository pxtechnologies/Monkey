namespace Monkey.Builder
{
    public class Field : IField
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public Field(string type, string name)
        {
            Type = type;
            Name = name;
        }
    };
}