using System;
using System.Collections.Generic;
using System.Linq;

namespace Monkey.Sql.Data
{
    public class SqlObjectTypeList
    {
        private readonly IObjectTypeList _instance;
        private readonly List<object[]> _list;
        private int _counter;
        public static readonly SqlObjectTypeList Instance = new SqlObjectTypeList(ObjectTypeList.Instance);
        private SqlObjectTypeList(IObjectTypeList instance)
        {
            _instance = instance;
            _list = new List<object[]>();
            _counter = 1;

            AddSqlTypes();
        }

        private void AddSqlTypes()
        {
            AddObject<string>("nvarchar","varchar","char","nchar","text","ntext");
            AddStruct<decimal>("numeric","decimal", "money", "smallmoney");

            AddStruct<int>("int")
                .AddStruct<short>("smallint")
                .AddStruct<byte>("tinyint")
                .AddStruct<long>("bigint");

            AddStruct<double>("float");
            AddStruct<float>("real");

            AddStruct<TimeSpan>("time");
            AddStruct<Guid>("uniqueidentifier");
            AddStruct<bool>("bit");

            AddStruct<DateTime>("date", "datetime", "datetime2", "smalldatetime");
            AddStruct<DateTimeOffset>("datetimeoffset");

            //TODO: missing: varbinary, binary, image, rowversion, timestamp, xml, sql_variant
        }

        public object[,] To2DArray()
        {
            return _list.ToArray2D();
        }
        private SqlObjectTypeList AddStruct<TObjectType>(params string[] sqlTypes)
            where TObjectType:struct
        {
            foreach (var sqlType in sqlTypes)
            {
                Type type = typeof(TObjectType);

                _list.Add(new object[]
                {
                    _counter++,
                    _instance.GetId<TObjectType?>(),
                    sqlType,
                    1
                });
                _list.Add(new object[]
                {
                    _counter++,
                    _instance.GetId<TObjectType>(),
                    sqlType,
                    0
                });

            }

            return this;
        }
        private SqlObjectTypeList AddObject<TObjectType>(params string[] sqlTypes)
            where TObjectType : class
        {
            foreach (var sqlType in sqlTypes)
            {
                Type type = typeof(TObjectType);

                _list.Add(new object[]
                {
                    _counter++,
                    _instance.GetId<TObjectType>(),
                    sqlType,
                    1
                });
            }

            return this;
        }

        public long[] GetIds()
        {
            return _list.Select(x => (long) x[0]).ToArray();
        }
    }
}