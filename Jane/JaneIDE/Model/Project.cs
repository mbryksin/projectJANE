using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaneIDE.Model
{
    class Project
    {
        private string projectName;
        private string projectFolderPath;
        private string authorName;
        private int projectVersion;
        private List<string> projectSources;

        public Project(string name, string path, string mainClass)
        {
            authorName = Environment.UserName;
            projectName = name;
            projectFolderPath = path;
            projectVersion = 100;

            projectSources = new List<string>();
            projectSources.Add(mainClass);
        }

        public string ProjectName
        {
            get { return projectName; }
        }

        public string Author
        {
            get { return authorName; }
        }

        public int ProjectVersion
        {
            get { return projectVersion; }
            set
            {
                if (value == projectVersion)
                    return;
                projectVersion = value;
            }
        }

        public string MainClass
        {
            get { return projectSources.First<string>(); }
        }

        public List<string> Sources
        {
            get { return projectSources; }
        }

        public void AddSource(string file)
        {
            if (projectSources.Contains(file))
                return;
            
            projectSources.Add(file);
        }

        public void SaveProject()
        {
            projectVersion += 1;
            Console.WriteLine("%s %s", this.ProjectName, this.MainClass);
        }

        public void RunProject()
        {

        }
    }
}
