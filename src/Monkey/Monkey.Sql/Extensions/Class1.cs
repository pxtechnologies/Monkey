using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Monkey.Sql.Extensions
{
    public static class ReaderExtensions
    {
        public static int IndexOf<TClass>(this TClass[] array, TClass search)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(search))
                    return i;
            }
            return -1;
        }
        public static int[] GetIndexes(this IDataReader rd, params string[] columns)
        {
            int[] result = new int[rd.FieldCount];

            for (int i = 0; i < result.Length; i++)
            {
                int index = columns.IndexOf(rd.GetName(i));
                result[i] = index;
            }


            return result;
        }
    }
}
