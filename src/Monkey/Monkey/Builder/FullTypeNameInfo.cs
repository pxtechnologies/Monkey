using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Monkey.Builder
{
    
    // A very simple structure for storing simple type information.
    public class FullTypeNameInfo
    {
        public static readonly Dictionary<Type, string> Aliases =
            new Dictionary<Type, string>()
            {
                { typeof(byte), "byte" },
                { typeof(sbyte), "sbyte" },
                { typeof(short), "short" },
                { typeof(ushort), "ushort" },
                { typeof(int), "int" },
                { typeof(uint), "uint" },
                { typeof(long), "long" },
                { typeof(ulong), "ulong" },
                { typeof(float), "float" },
                { typeof(double), "double" },
                { typeof(decimal), "decimal" },
                { typeof(object), "object" },
                { typeof(bool), "bool" },
                { typeof(char), "char" },
                { typeof(string), "string" },
                { typeof(void), "void" }
            };
        
        private string _namespace;
        private string _name;
        private bool _isGeneric;
        public override string ToString()
        {
            if (!IsGeneric)
            {
                if (Namespace == "System")
                {
                    //TODO clean up
                    var alias = Aliases.Where(x => x.Key.Name == _name)
                        .Select(x=>x.Value)
                        .FirstOrDefault();

                    if (alias != null)
                        return alias;

                }
                return _name;
            }
            else
            {
                if (IsNullable)
                    return $"{_genericArguments[0]}?";
                return $"{_name}<{string.Join(", ", _genericArguments)}>";
            }
        }

        public string FullName
        {
            get { return $"{_namespace}.{ToString()}"; }
        }
        private List<FullTypeNameInfo> _genericArguments;

        protected FullTypeNameInfo()
        {
            _genericArguments = new List<FullTypeNameInfo>();
        }
        public static explicit operator FullTypeNameInfo(Type t)
        {
            return FullTypeNameInfo.Parse(t.FullName);
        }
        public static implicit operator FullTypeNameInfo(string t)
        {
            return FullTypeNameInfo.Parse(t);
        }
        public IEnumerable<FullTypeNameInfo> GenericArguments => _genericArguments;
        public string Name => _name;
        public string Namespace => _namespace;

        public bool IsNullable
        {
            get { return Name == "Nullable";  }
        }
        public bool IsNamespaceDefined => !string.IsNullOrWhiteSpace(Namespace);

        public bool IsGeneric
        {
            get { return _isGeneric; }
            private set { _isGeneric = value; }
        }

        public static FullTypeNameInfo Parse(string fullType)
        {
            FullTypeNameInfo result = new FullTypeNameInfo();
            Parse(result, fullType.Replace(" ",""));
            return result;
        }
        private static int Parse(FullTypeNameInfo result, string fullType, int startIndex = 0)
        {
            
            int lastDot = 0;
            int lastComma = 0;
            for (int i = startIndex; i < fullType.Length; i++)
            {
                switch (fullType[i])
                {
                    case '[':   // this will be a generic
                        if (fullType[i + 1] == '[') i = i + 1;  // start of arguments
                        var gArg = new FullTypeNameInfo();
                        result._genericArguments.Add(gArg);
                        result.IsGeneric = true;
                        startIndex = i = Parse(gArg, fullType, i + 1);
                        break;
                    case '.':
                        lastDot = i;
                        break;
                    case ',':
                        if(result._name == null) // this is full-type-name
                            ParseOnlyName(result, fullType, startIndex, lastDot, i-1);
                        lastComma = i;
                        // we don't parse assembly name, version or culture.
                        break;
                    case ']': // this is end of generic
                        return i;
                    case '`':
                        ParseOnlyName(result, fullType, startIndex, lastDot, i-1);
                        break;
                    default:
                        continue;
                }
            }

            if (result._name == null)
            {
                ParseOnlyName(result, fullType, startIndex, lastDot, fullType.Length-1);
            }

            return fullType.Length;
        }

        private static void ParseOnlyName(FullTypeNameInfo result, string text, int start, int dot, int end)
        {
            if (dot > 0)
            {
                result._namespace = text.Substring(start, dot - start);
                result._name = text.Substring(dot + 1, end - dot);
            }
            else
            {
                result._name = text;
            }
        }

        
    }
    public static class Extensions {
        public static FullTypeNameInfo GetFullTypeInfo(this Type t)
        {
            return ParseType(t.FullName);
        }
        public static FullTypeNameInfo ParseType(this string str)
        {
            return FullTypeNameInfo.Parse(str);
        }
    }
}