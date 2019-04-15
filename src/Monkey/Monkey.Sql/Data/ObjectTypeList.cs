using System;
using System.Collections.Generic;
using System.Linq;
using Monkey.Builder;

namespace Monkey.Sql.Data
{
    public interface IObjectTypeList
    {
        long GetId<TType>();
    }

    public class ObjectTypeList : IObjectTypeList
    {
        public static ObjectTypeList Instance = new ObjectTypeList();
        
        public long[] GetIds()
        {
            return _list.Select(x => (long)x[0]).ToArray();
        }
        private string GetAlias<TType>()
        {
            var type = typeof(TType);
            string sufix = "";
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
                sufix = "?";
            }

            if (FullTypeNameInfo.Aliases.ContainsKey(type))
                return FullTypeNameInfo.Aliases[type] + sufix;
            return null;
        }
        private ObjectTypeList()
        {
            _list = new List<object[]>();
            _counter = 1;
            _ids = new Dictionary<Type, long>();

            AddBcl();
        }

        private void AddBcl()
        {
            AddObject<string>();
            //AddObject<byte[]>();

            AddStruct<short>()
                .AddStruct<int>()
                .AddStruct<long>();

            AddStruct<float>()
                .AddStruct<double>();

            AddStruct<decimal>();
            AddStruct<Guid>();
            AddStruct<bool>();
            AddStruct<byte>();
            AddStruct<char>();
            

            AddStruct<TimeSpan>()
                .AddStruct<DateTime>()
                .AddStruct<DateTimeOffset>();
        }

        private readonly Dictionary<Type, long> _ids;
        private readonly List<object[]> _list;
        private long _counter;
        public long GetId<TType>()
        {
            return _ids[typeof(TType)];
        }

        private ObjectTypeList AddObject<TType>() where TType : class
        {
            var type = typeof(TType);
            var id = _counter++;
            _list.Add(new object[]
            {
                id,
                type.Name,
                type.Namespace,
                "BCL",
                true,
                GetAlias<TType>(),
                0, 0
            });
            _ids.Add(type, id);
            return this;
        }

        private ObjectTypeList AddStruct<TType>() where TType:struct
        {
            var type = typeof(TType);
            var id = _counter++;
            _list.Add(new object[]{ id,
                type.Name,
                type.Namespace,
                "BCL",
                true,
                GetAlias<TType>(),
                0,0
            });
            _ids.Add(type, id);

            type = typeof(Nullable<TType>);
            FullTypeNameInfo ft = FullTypeNameInfo.Parse(type.FullName);
            id = _counter++;
            _list.Add(new object[]{ id,
                ft.ToString(),
                ft.Namespace,
                "BCL",
                true,
                GetAlias<TType?>(),
                0,0
            });
            _ids.Add(type, id);
            return this;
        }

        public object[,] To2DArray()
        {
            return _list.ToArray2D();
        }
    }
}