using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaneIDE.Model
{
    class Project
    {
        string projectName;
        string projectFolderPath;
        string authorName;
        int projectVersion;
        List<string> projectSources;

        public Project(string name, string path, string mainClass)
        {
            authorName = Environment.UserName;
            projectName = name;
            projectFolderPath = path;
            projectSources.Add(mainClass);
            projectVersion = 100;
        }

        public void AddSource(string file)
        {
            if (!projectSources.Contains(file))
                projectSources.Add(file);
        }

        public void SaveProject()
        {

        }

        public void RunProject()
        {

        }

        public List<string> Sources
        {
            get { return projectSources; }
        }

    }
}
