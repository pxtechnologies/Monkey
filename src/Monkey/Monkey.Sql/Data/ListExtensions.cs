using System.Collections.Generic;

namespace Monkey.Sql.Data
{
    public static class ListExtensions
    {
        public static T[,] ToArray2D<T>(this List<T[]> list)
        {
            T[,] result = new T[list.Count, list[0].Length];
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Length; j++)
                    result[i, j] = list[i][j];

            }

            return result;
        } 
    }
}