using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;


namespace Monkey.Compilation
{
    public interface ITypeCompiler
    {
        Type FastLoad(string code, string ns, string className);
        Assembly FastLoad(string code);
    }
    public class TypeCompiler : ITypeCompiler
    {
        public Type FastLoad(string code, string ns, string className)
        {
            return FastLoad(code).GetType($"{ns}.{className}"); 
        }
        public Assembly FastLoad(string code)
        {
            string assemblyName = Path.GetRandomFileName();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            MetadataReference[] references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic)
                .Select(x => MetadataReference.CreateFromFile(x.Location))
                .ToArray();


            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    StringBuilder sb = new StringBuilder();
                    foreach (Diagnostic diagnostic in failures)
                    {
                        sb.AppendFormat("{0}: {1}\r\n", diagnostic.Id, diagnostic.GetMessage());
                    }
                    throw new CompilationException(sb.ToString()) { Errors = failures.ToArray() };
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    return assembly;
                }
            }
        }
    }
}