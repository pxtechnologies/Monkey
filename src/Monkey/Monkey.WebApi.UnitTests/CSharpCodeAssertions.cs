using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Monkey.Cqrs;
using NSubstitute;
using Xunit;

namespace Monkey.WebApi.UnitTests
{
    public static class CSharpCodeAssertions
    {
        public static void CodeParses(string code)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(string.Join(Environment.NewLine, code));
            
            string[] lines = Regex.Split(code, "\r\n|\r|\n");
            StringBuilder sb = new StringBuilder("");
            var diagnostics = tree.GetDiagnostics().ToArray();
            if (diagnostics.Any())
                sb.AppendLine($"Found {diagnostics.Length} errors:");

            foreach (Diagnostic diagnostic in diagnostics)
            {
                sb.AppendFormat("{0}: {1}\r\n", diagnostic.Id, diagnostic.GetMessage());
                var line = diagnostic.Location.GetLineSpan().StartLinePosition.Line;
                sb.AppendFormat($"Code({line}):\r\n");

                for (int i = line - 1; i < line + 2; i++)
                {
                    sb.AppendLine($"{i}:{lines[i]}");
                }

                sb.AppendLine();
            }
            if(!String.IsNullOrWhiteSpace(sb.ToString()))
                throw new AssertionFailedException(sb.ToString());
        }
    }

    
}