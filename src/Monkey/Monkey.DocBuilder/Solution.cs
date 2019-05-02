using System.Collections.Generic;

namespace Monkey.DocBuilder
{
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
}