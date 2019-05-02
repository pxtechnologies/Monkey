using System;
using Gherkin.Pickles;
using Xunit;

namespace Monkey.DocBuilder
{
    public class DocBuilder
    {
        [Fact]
        public void Build()
        {
            Solution s = new Solution().WithProjectFrom(@"F:\src\Monkey\src\Monkey\Monkey.Sql.WebApiHost.AcceptanceTests\");
            
            MarkupWriter writer = new MarkupWriter(@"F:\src\Monkey\src\Monkey\Docs");

            writer.Write(s);
        }
    }
}
