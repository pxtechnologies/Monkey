using System.Linq;

namespace Monkey.Sql.WebApiHost.AcceptanceTests.SharedSteps
{
    public static class StrExt
    {
        public static string RemoveWhiteSpaces(this string str)
        {
            return new string(str.Where(x => !char.IsWhiteSpace(x)).ToArray());
        }
    }
}