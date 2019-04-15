using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Monkey.Sql.Builder
{
    sealed class SqlReaderMethodDictionary : ISqlReaderMethodDictionary
    {
        private readonly Dictionary<Type, string> _dictionary;
        public SqlReaderMethodDictionary()
        {
            _dictionary = new Dictionary<Type, string>();
            _dictionary.Add(typeof(string), nameof(IDataRecord.GetString));
            AddStruct<int>( nameof(IDataRecord.GetInt32));
            AddStruct<byte>( nameof(IDataRecord.GetByte));
            AddStruct<short>( nameof(IDataRecord.GetInt16));
            AddStruct<bool>( nameof(IDataRecord.GetBoolean));
            AddStruct<DateTime>( nameof(IDataRecord.GetDateTime));
            AddStruct<float>( nameof(IDataRecord.GetFloat));
            AddStruct<long>( nameof(IDataRecord.GetInt64));
            AddStruct<DateTimeOffset>( nameof(SqlDataReader.GetDateTimeOffset));
            AddStruct<Decimal>( nameof(SqlDataReader.GetDecimal));
            AddStruct<TimeSpan>( nameof(SqlDataReader.GetTimeSpan));
            AddStruct<Double>( nameof(SqlDataReader.GetDouble));

        }

        private SqlReaderMethodDictionary AddStruct<TStruct>(string readerMthName)
        where TStruct:struct
        {
            var type = typeof(TStruct);
            _dictionary.Add(type, readerMthName);
            _dictionary.Add(typeof(Nullable<>).MakeGenericType(type), readerMthName);
            return this;
        }
        public string this[Type type]
        {
            get { return _dictionary[type]; }
        }
    }
}