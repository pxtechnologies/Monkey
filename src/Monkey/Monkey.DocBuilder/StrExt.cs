namespace Monkey.DocBuilder
{
    static class StrExt
    {
        public static string ToMarkup(this string ghiernkin)
        {
            return ghiernkin.Replace("<", "**\\<")
                .Replace(">", "\\>**");
        }
    }
}