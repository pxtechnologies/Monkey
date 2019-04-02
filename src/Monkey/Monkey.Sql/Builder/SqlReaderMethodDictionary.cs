using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Monkey.Sql.Builder
{
    sealed class SqlReaderMethodDictionary : ISqlReaderMethodDictionary
    {
        private readonly Dictionary<string, string> _dictionary;
        public SqlReaderMethodDictionary()
        {
            _dictionary = new Dictionary<string, string>();
            _dictionary.Add("string", nameof(IDataReader.GetString));
            _dictionary.Add("int", nameof(IDataReader.GetInt32));
            _dictionary.Add("bool", nameof(IDataReader.GetBoolean));
            _dictionary.Add("DateTime", nameof(IDataReader.GetDateTime));
            _dictionary.Add("float", nameof(IDataReader.GetFloat));
            _dictionary.Add("long", nameof(IDataReader.GetInt64));
            _dictionary.Add("DateTimeOffset", nameof(SqlDataReader.GetDateTimeOffset));
            _dictionary.Add("Decimal", nameof(SqlDataReader.GetDecimal));
            _dictionary.Add("Double", nameof(SqlDataReader.GetDouble));

        }
        public string this[string type]
        {
            get { return _dictionary[type]; }
        }
    }
}