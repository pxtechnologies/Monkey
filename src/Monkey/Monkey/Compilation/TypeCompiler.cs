using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Monkey.Cqrs;
using NSubstitute;


namespace Monkey.Compilation
{
    public interface ITypeCompiler
    {
        Type FastLoad(string code, string ns, string className, params Assembly[] references);
        Assembly FastLoad(string code, params Assembly[] references);
    }
    public class TypeCompiler : ITypeCompiler
    {
        public Type FastLoad(string code, string ns, string className, params Assembly[] references)
        {
            return FastLoad(code, references).GetType($"{ns}.{className}"); 
        }
        public Assembly FastLoad(string code, params Assembly[] rs)
        {
            string assemblyName = Path.GetRandomFileName();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            
            MetadataReference[] references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic)
                .Union(rs)
                .Distinct()
                .Select(OnLoadAssembly)
                .Where(x=> x != null)
                .ToArray();


            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel:OptimizationLevel.Release)
                );

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);
                    string[] lines = Regex.Split(code, "\r\n|\r|\n");
                    StringBuilder sb = new StringBuilder();
                    foreach (Diagnostic diagnostic in failures)
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
                    throw new CompilationException(sb.ToString()) { Errors = failures.ToArray() };
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    string tmpFile = Path.GetTempFileName() + ".dll";
                    using (var file = File.OpenWrite(tmpFile))
                        ms.CopyTo(file);

                    Assembly assembly = Assembly.LoadFrom(tmpFile);
                    return assembly;
                }
            }
        }

        private PortableExecutableReference OnLoadAssembly(Assembly x)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(x.Location))
                {
                    return null;   
                }
                return MetadataReference.CreateFromFile(x.Location);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
        }
    }
}