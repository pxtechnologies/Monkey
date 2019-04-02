using System;
using System.Security.Cryptography;
using System.Text;

namespace Monkey
{
    public static class StrExt
    {
        public static Guid ComputeMd5(this string str)
        {
            using (var h = new MD5CryptoServiceProvider())
            {
                h.Initialize();
                var result = h.ComputeHash(Encoding.Default.GetBytes(str));
                return new Guid(result);
            }
        }
        public static string FindNamespace(this string str)
        {
            int index = str.LastIndexOf(".");
            if (index > 0)
                return str.Remove(index);
            return "";
        }
        public static string FindTypeName(this string str)
        {
            int index = str.LastIndexOf(".");
            if (index > 0)
                return str.Substring(index + 1);
            return str;
        }

        public static string DblQuoted(this string str)
        {
            return $"\"{str}\"";
        }
        public static string SqlQuoted(this string str)
        {
            return $"[{str}]";
        }
        public static string Quoted(this string str)
        {
            return $"'{str}'";
        }
        public static string StartLower(this string str)
        {
            if (!char.IsLower(str[0]))
            {
                return char.ToLower(str[0]) + str.Substring(1);
            }

            return str;
        }
    }
}