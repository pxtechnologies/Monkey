namespace Monkey.Sql.Builder
{
    public interface ISqlReaderMethodDictionary
    {
        string this[string type] { get; }
    }
}