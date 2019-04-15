using System;

namespace Monkey.Sql.Builder
{
    public interface ISqlReaderMethodDictionary
    {
        string this[Type type] { get; }
    }
}