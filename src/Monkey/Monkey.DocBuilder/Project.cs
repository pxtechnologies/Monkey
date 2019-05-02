using System.Collections.Generic;
using System.IO;
using Gherkin;
using Gherkin.Ast;

namespace Monkey.DocBuilder
{
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