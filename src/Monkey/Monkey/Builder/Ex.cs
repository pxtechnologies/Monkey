namespace Monkey.Builder
{
    public static class Ex
    {
        public static string GenerateCode<T>(this T builder)
            where T : ICodeBlockBuilder
        {
            SourceCodeBuilder sb = new SourceCodeBuilder();
            builder.GenerateCode(sb);
            return sb.ToString();
        }
    }
}