using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gherkin;
using Gherkin.Ast;
using Gherkin.Pickles;
using Humanizer;
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

    static class StrExt
    {
        public static string ToMarkup(this string ghiernkin)
        {
            return ghiernkin.Replace("'<", "**\"")
                .Replace(">'", "\"**");
        }
    }
    public class MarkupWriter
    {
        private readonly string _dstPath;

        public MarkupWriter(string dstPath)
        {
            _dstPath = dstPath;
        }

        public void Write(Solution sln)
        {
            Write(sln.Projects.SelectMany(x => x.Features));
        }

        private void Write(IEnumerable<Feature> features)
        {
            foreach(var f in features) Write(f);;
        }

        private void Write(Feature feature)
        {
            var dstFile = Path.Combine(_dstPath, feature.Name + ".md");
            StringBuilder sb =  new StringBuilder();
            sb.AppendLine($"# {feature.Name.Humanize()}");
            sb.AppendLine();

            if (feature.Children.OfType<Background>().Any())
                sb.AppendLine("## Background: ");

            foreach (var s in feature.Children.OfType<Background>())
            {
                if(s.Name != null)
                    sb.AppendLine($"## {s.Name}");
                if(s.Description != null)
                    sb.AppendLine(s.Description);
                foreach (var step in s.Steps)
                {
                    WriteStep(sb, step);
                }
            }

            foreach (var s in feature.Children.OfType<Scenario>())
            {
                sb.AppendLine($"## {s.Name}");
                if(s.Description != null)
                    sb.AppendLine(s.Description);

                foreach (var step in s.Steps)
                {
                    WriteStep(sb, step);
                }

                if (s.Examples.Any())
                {
                    sb.AppendLine("### Examples:");
                    foreach (var e in s.Examples)
                    {
                        WriteExample(sb, e);
                    }
                }
            }

            var dir = Path.GetDirectoryName(dstFile);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var prv = File.Exists(dstFile) ? File.ReadAllText(dstFile) : "";
            if(prv != sb.ToString())
                File.WriteAllText(dstFile, sb.ToString());
        }

        private static void WriteExample(StringBuilder sb, Examples e)
        {
            sb.Append("| ");
            sb.Append(string.Join(" | ", e.TableHeader.Cells.Select(x => x.Value.Humanize())));
            sb.Append("| ");
            sb.AppendLine();
            sb.Append("| ");
            sb.Append(string.Join(" | ", e.TableHeader.Cells.Select(x => "---")));
            sb.Append("| ");
            sb.AppendLine();
            foreach (var r in e.TableBody)
            {
                sb.Append("| ");
                sb.Append(string.Join(" | ", r.Cells.Select(x => x.Value.ToMarkup())));
                sb.Append("| ");
                sb.AppendLine();
            }
        }

        private static StringBuilder WriteStep(StringBuilder sb, Step step)
        {
            sb.AppendLine($"**_{step.Keyword.TrimEnd(' ')}_** {step.Text.ToMarkup()}<br />");
            if (step.Argument is DataTable)
            {
                DataTable tArg = (DataTable) step.Argument;
                if (tArg.Rows.First().Cells.Count() == 1)
                {
                    // this should be code-snippet
                    string lang = tArg.Rows.First().Cells.First().Value;
                    sb.AppendLine($"```{lang}");
                    foreach (var r in tArg.Rows.Skip(1))
                        sb.AppendLine(r.Cells.First().Value);
                    sb.AppendLine($"```");
                }
                else
                {
                    bool isHeader = true;
                    foreach (var r in tArg.Rows)
                    {
                        sb.Append("| ");
                        if (isHeader)
                            sb.Append(string.Join(" | ", r.Cells.Select(x => x.Value.Humanize())));
                        else
                            sb.Append(string.Join(" | ", r.Cells.Select(x => x.Value.ToMarkup())));
                        sb.Append(" |");
                        sb.AppendLine();
                        if (isHeader)
                        {
                            isHeader = false;
                            sb.Append("| ");
                            sb.Append(string.Join(" | ", r.Cells.Select(x => "---")));
                            sb.Append(" |");
                        }
                    }
                }
            }
            else if (step.Argument is DocString)
            {
                DocString str = (DocString) step.Argument;
                // we don't know
            }

            return sb;
        }
    }
    public class Solution
    {
        public List<Project> Projects { get; private set; }

        public Solution()
        {
            Projects = new List<Project>();
        }

        public Solution WithProjectFrom(string path)
        {
            Project p = new Project(path);
            p.Discover();
            Projects.Add(p);
            return this;
        }
    }
    public class Project
    {
        private List<Feature> _features;
        private string _rootPath;
        public IList<Feature> Features => _features;
        public Project(string rootPath)
        {
            _features = new List<Feature>();
            _rootPath = rootPath;
        }
        public Project Discover()
        {
            var srcFiles = Directory.GetFiles(_rootPath, "*.feature", SearchOption.AllDirectories);
            foreach (var f in srcFiles)
            {
                var parser = new Parser();
                var gherkinDocument = parser.Parse(f);
                
                _features.Add(gherkinDocument.Feature);
            }

            return this;
        }
    }

    
}
