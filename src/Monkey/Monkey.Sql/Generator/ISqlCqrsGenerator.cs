using System.Threading.Tasks;
using Monkey.Generator;

namespace Monkey.Sql.Generator
{
    public interface ISqlCqrsGenerator
    {
        Task<SourceUnitCollection> Generate(long version);
    }
}