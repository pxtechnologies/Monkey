using System;
using System.Collections.Generic;
using System.Text;
using Monkey.Builder;

namespace Monkey.Extensions
{
    public static class Ex
    {
        public static string GenerateCode<T>(this T builder)
            where T: ICodeBlockBuilder
        {
            SourceCodeBuilder sb = new SourceCodeBuilder();
            builder.GenerateCode(sb);
            return sb.ToString();
        }
    }
}
