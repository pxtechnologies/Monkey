namespace Monkey.Sql.Builder
{
    public interface ICodeBlockBuilder
    {
        void GenerateCode(SourceCodeBuilder sb);
    }
}