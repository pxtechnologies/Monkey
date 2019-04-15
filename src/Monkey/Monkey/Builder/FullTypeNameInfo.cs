using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Monkey.Builder
{
    public static class FullTypeNameInfoExtensions
    {
        public static Type GetPrimitiveType(this FullTypeNameInfo info)
        {
            if(info.Namespace != "System") throw new NotSupportedException();
            if (info.IsNullable)
            {
                Type inner = info.GenericArguments.First().GetPrimitiveType();
                return typeof(Nullable<>).MakeGenericType(inner);
            }
            else
            {
                switch (info.Name)
                {
                    case "String":
                        return typeof(String);
                    case "Int16":
                        return typeof(Int16);
                    case "Int32":
                        return typeof(Int32);
                    case "Int64":
                        return typeof(Int64);
                    case "Single":
                        return typeof(Single);
                    case "Double":
                        return typeof(Double);
                    case "Decimal":
                        return typeof(Decimal);
                    case "Guid":
                        return typeof(Guid);
                    case "Boolean":
                        return typeof(Boolean);
                    case "Byte":
                        return typeof(Byte);
                    case "Char":
                        return typeof(Char);
                    case "TimeSpan":
                        return typeof(TimeSpan);
                    case "DateTime":
                        return typeof(DateTime);
                    case "DateTimeOffset":
                        return typeof(DateTimeOffset);
                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
    // A very simple structure for storing simple type information.
    public class FullTypeNameInfo
    {
        public static readonly Dictionary<Type, string> Aliases =
            new Dictionary<Type, string>()
            {
                {typeof(byte), "byte"},
                {typeof(sbyte), "sbyte"},
                {typeof(short), "short"},
                {typeof(ushort), "ushort"},
                {typeof(int), "int"},
                {typeof(uint), "uint"},
                {typeof(long), "long"},
                {typeof(ulong), "ulong"},
                {typeof(float), "float"},
                {typeof(double), "double"},
                {typeof(decimal), "decimal"},
                {typeof(object), "object"},
                {typeof(bool), "bool"},
                {typeof(char), "char"},
                {typeof(string), "string"},
                {typeof(void), "void"}
            };

        private string _namespace;
        private string _name;
        private bool _isGeneric;

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool shortForm = true, bool fullName = false)
        {
            if (!IsGeneric)
            {
                if (Namespace == "System")
                {
                    //TODO clean up
                    var alias = Aliases.Where(x => x.Key.Name == _name)
                        .Select(x => x.Value)
                        .FirstOrDefault();

                    if (alias != null && !fullName)
                        return alias;

                }

                if (fullName && _namespace != null)
                    return $"{_namespace}.{_name}";
                return _name;
            }
            else
            {
                if (IsNullable && shortForm)
                    return $"{_genericArguments[0].ToString(shortForm, fullName)}?";

                var args = string.Join(", ", _genericArguments.Select(i => i.ToString(shortForm, fullName)));
                if (fullName && _namespace != null)
                    return $"{_namespace}.{_name}<{args}>";
                return $"{_name}<{args}>";
            }
        }

        public string FullName
        {
            get { return $"{_namespace}.{ToString(false)}"; }
        }

        private List<FullTypeNameInfo> _genericArguments;

        protected FullTypeNameInfo()
        {
            _genericArguments = new List<FullTypeNameInfo>();
        }

        public static implicit operator FullTypeNameInfo(Type t)
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
            get { return Name == "Nullable"; }
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
            if (fullType.Contains('<'))
                ParseAsCSharpCode(result, fullType.Replace(" ", ""));
            else
                ParseAsFQN(result, fullType.Replace(" ", ""));
            return result;
        }

        private static int ParseAsCSharpCode(FullTypeNameInfo result, string fullType, int startIndex = 0)
        {
            int lastDot = 0;
            for (int i = startIndex; i < fullType.Length; i++)
            {
                switch (fullType[i])
                {
                    case '<': // this will be a generic
                        var gArg = new FullTypeNameInfo();
                        result._genericArguments.Add(gArg);
                        result.IsGeneric = true;
                        ParseOnlyName(result, fullType, startIndex, lastDot, i - 1);
                        startIndex = i = ParseAsCSharpCode(gArg, fullType, i + 1);
                        break;
                    case '.':
                        lastDot = i;
                        break;
                    
                    case '>': // this is end of generic
                        ParseOnlyName(result, fullType, startIndex, lastDot, i-1);
                        return i;
                    
                    default:
                        continue;
                }
            }

            if (result._name == null)
            {
                ParseOnlyName(result, fullType, startIndex, lastDot, fullType.Length - 1);
            }

            return fullType.Length;


        }

        private static int ParseAsFQN(FullTypeNameInfo result, string fullType, int startIndex = 0)
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
                        startIndex = i = ParseAsFQN(gArg, fullType, i + 1);
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