using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

        public Project()
        {
            projectSources = new List<string>();
        }

        public Project(string name, string path, string mainClass): this()
        {
            this.Author = Environment.UserName;
            this.ProjectName = name;
            this.ProjectFolder = path;
            this.Version = 100;

            this.Sources.Add(mainClass);
        }

        public string Author
        {
            get { return authorName; }
            private set { authorName = value; }
        }

        public string ProjectName
        {
            get { return projectName; }
            private set { projectName = value; } 
        }

        public string ProjectFolder
        {
            get { return projectFolderPath; }
            private set { projectFolderPath = value; }
        }

        public int Version
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
            if (!Directory.Exists(this.ProjectFolder))
                Directory.CreateDirectory(this.ProjectFolder);

            string projectFilePath = Path.Combine(this.ProjectFolder, this.ProjectName);
            if (!projectFilePath.EndsWith(".pro"))
                projectFilePath += ".pro";
            
            string manifestPath = Path.Combine(this.ProjectFolder, "MANIFEST.MF");

            try
            {
                //.pro

                if (File.Exists(projectFilePath))
                {
                    File.Delete(projectFilePath);
                }
                
                using (FileStream fs = File.Create(projectFilePath))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(this.ProjectFolder);
                    fs.Write(info, 0, info.Length);
                }

                //.MF

                if (File.Exists(manifestPath))
                {
                    File.Delete(manifestPath);
                }

                using (FileStream fs = File.Create(manifestPath))
                {
                    string manifestText = System.String.Empty;
                    manifestText += "Author: " + this.Author + "\r\n";
                    manifestText += "Version: " + this.Version.ToString() + "\r\n";
                    manifestText += "Main-Class: " + this.MainClass + "\r\n";
                    manifestText += "Sources:\r\n";
                    foreach (string source in this.Sources)
                    {
                        if (source.EndsWith(".jane"))
                            manifestText += source + "\r\n";
                        else
                            manifestText += source + ".jane\r\n";
                    }

                    Byte[] info = new UTF8Encoding(true).GetBytes(manifestText);
                    fs.Write(info, 0, info.Length);
                }

                //.jane

                foreach (string source in this.Sources)
                {
                    string sourcePath = Path.Combine(this.ProjectFolder, source);
                    if (!sourcePath.EndsWith(".jane"))
                        sourcePath += ".jane";

                    if (File.Exists(sourcePath))
                    {
                        File.Delete(sourcePath);
                    }

                    File.Create(sourcePath);
                }
            }

            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.ToString());
            }

        }

        public void OpenProject(string openedProjectName, string openedProjectPath)
        {
            string[] split = openedProjectName.Split(new Char[] { '\\', '\t', '\n' });
            string pro = split.Last<string>();
            if (!pro.EndsWith(".pro"))
            {
                return;
            }
            string[] namesplit = pro.Split(new Char[] { '.' });
            
            this.ProjectName = namesplit.First<string>();
            this.ProjectFolder = openedProjectPath;

            string manifestPath = Path.Combine(openedProjectPath, "MANIFEST.MF");

            try
            {
                if (!File.Exists(manifestPath))
                { return; }
                
                using (FileStream fs = File.Open(manifestPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Debug.WriteLine(line);
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.ToString());
            }

        }

        public void RunProject()
        {

        }
    }
}
