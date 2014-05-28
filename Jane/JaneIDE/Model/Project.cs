using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using RunProject;
using AST;
using JaneRuntime;

namespace JaneIDE.Model
{
    class Project
    {
        private const int DEFAULT_VERSION = 100;
        private const string SOURCE_FORMAT = ".jane";
        private const string PROJECT_FORMAT = ".pro";
        private const string MANIFEST_FILENAME = "MANIFEST.MF";

        
        public event EventHandler<String> OutputWriteLineEvent;
        public event EventHandler<String> OutputWriteEvent;
        public event EventHandler<String> ErrorsWriteLineEvent;
        public event EventHandler<String> ProcessWriteLineEvent;

        private bool canRun;
        private bool canSave;

        private string projectName;
        private string projectFolderPath;
        private string authorName;
        private int projectVersion;
        private List<Source> projectSources;

        public Project()
        {   
            projectSources = new List<Source>();
            this.CanRun = false;
            this.CanSave = false;

            JaneRuntime.ConsoleEvent.Event += ConsoleWriteLine;
        }

        void ConsoleWriteLine(object sender, String arg)
        {
            OutputWriteLineEvent(this, arg);
        }

        private void SortOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            // Collect the sort command output. 
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                OutputWriteLineEvent(this, outLine.Data);
            }
        }

        public void SetProject(string name, string path, string mainClass)
        {
            this.Author = Environment.UserName;
            
            this.ProjectName = name;   
            this.ProjectFolder = path;
            this.Version = DEFAULT_VERSION;

            this.AddSource(new Source(mainClass));
            this.CanRun = true;
            this.CanSave = true;
        }

        private void CleanProject()
        {
            this.ProjectName = String.Empty;
            this.ProjectFolder = String.Empty;
            this.CanRun = false;
            this.CanSave = false;
            projectSources.Clear();
            this.Version = DEFAULT_VERSION;
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
        public bool CanRun
        {
            get { return canRun; }
            set
            {
                if (value == canRun)
                    return;
                canRun = value;
            }
        }
        public bool CanSave
        {
            get { return canSave; }
            set
            {
                if (value == canSave)
                    return;
                canSave = value;
            }
        }
        public Source MainClass
        {
            get { return projectSources.First<Source>(); }
        }
        public List<Source> Sources
        {
            get { return projectSources; }
        }
        public void AddSource(Source src)
        {
            if (projectSources.Contains(src))
                return;

            projectSources.Add(src);
        }
        public void SaveProject()
        {
            if (!this.CanSave)
                return;

            projectVersion += 1;

            if (!Directory.Exists(this.ProjectFolder))
                Directory.CreateDirectory(this.ProjectFolder);

            string projectFilePath = Path.Combine(this.ProjectFolder, this.ProjectName);
            if (!projectFilePath.EndsWith(PROJECT_FORMAT))
                projectFilePath += PROJECT_FORMAT;

            string manifestPath = Path.Combine(this.ProjectFolder, MANIFEST_FILENAME);

            try
            {
                //*.pro

                if (File.Exists(projectFilePath))
                {
                    File.Delete(projectFilePath);
                }
                
                using (FileStream fs = File.Create(projectFilePath))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(this.ProjectFolder);
                    fs.Write(info, 0, info.Length);
                }

                //MANIFEST.MF

                if (File.Exists(manifestPath))
                {
                    File.Delete(manifestPath);
                }

                using (FileStream fs = File.Create(manifestPath))
                {
                    string manifestText = String.Empty;
                    manifestText += "Author: " + this.Author + "\r\n";
                    manifestText += "Version: " + this.Version.ToString() + "\r\n";
                    manifestText += "Main-Class: " + this.MainClass.FileName + "\r\n";
                    manifestText += "Sources:\r\n";

                    foreach (Source source in this.Sources)
                    {
                        if (source.FileName.EndsWith(SOURCE_FORMAT))
                            manifestText += source.FileName + "\r\n";
                        else
                            manifestText += source.FileName + SOURCE_FORMAT + "\r\n";
                    }

                    Byte[] info = new UTF8Encoding(true).GetBytes(manifestText);
                    fs.Write(info, 0, info.Length);
                }

                //*.jane

                foreach (Source source in this.Sources)
                {
                    string sourcePath = Path.Combine(this.ProjectFolder, source.FileName);
                    if (!sourcePath.EndsWith(SOURCE_FORMAT))
                        sourcePath += SOURCE_FORMAT;

                    if (File.Exists(sourcePath))
                    {
                        File.Delete(sourcePath);
                    }

                    using (FileStream fs = File.Create(sourcePath))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(source.Content);
                        fs.Write(info, 0, info.Length);
                    }
                    
                }

                this.CanRun = true;
                this.ProcessWriteLineEvent(this, this.ProjectName + ": saved");
            }

            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.ToString());
            }

        }
        public void OpenProject(string openedProjectName, string openedProjectPath)
        {
            this.CleanProject();

            string[] split = openedProjectName.Split(new Char[] { '\\', '\t', '\n' });
            string pro = split.Last<string>();
            if (!pro.EndsWith(PROJECT_FORMAT))
            {
                return;
            }
            string[] namesplit = pro.Split(new Char[] { '.' });
            
            this.ProjectName = namesplit.First<string>();
            this.ProjectFolder = openedProjectPath;

            string manifestPath = Path.Combine(openedProjectPath, MANIFEST_FILENAME);

            try
            {
                if (!File.Exists(manifestPath))
                { return; }
                
                using (FileStream fs = File.Open(manifestPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        //Author
                        string line = sr.ReadLine();
                        string[] linesplit = line.Split(new Char[] { ' ', ':', '\t', '\n' });
                        this.Author = linesplit.Last<string>().Trim();
                        
                        //Version
                        line = sr.ReadLine();
                        linesplit = line.Split(new Char[] { ' ', ':', '\t', '\n' });
                        this.Version = Convert.ToInt32(linesplit.Last<string>().Trim());
                        
                        //Main-Class
                        line = sr.ReadLine();
                        linesplit = line.Split(new Char[] { ' ', ':', '\t', '\n' });
                        string mainClassFileName = linesplit.Last<string>().Trim();
                        
                        Source mainClassSource = new Source(mainClassFileName);

                        string mainClassFilePath = Path.Combine(this.ProjectFolder, mainClassFileName);
                        if (!mainClassFilePath.EndsWith(SOURCE_FORMAT))
                            mainClassFilePath += SOURCE_FORMAT;

                        /*
                        if (File.Exists(mainClassFilePath))
                        {
                            File.Delete(mainClassFilePath);
                        }
                        */

                        using (FileStream fstream = File.Open(mainClassFilePath, FileMode.Open))
                        {
                            using (StreamReader sreader = new StreamReader(fstream))
                            {
                                mainClassSource.Content = sreader.ReadToEnd().Trim();
                            }
                        }

                        this.AddSource(mainClassSource);
                        //Sources
                        sr.ReadLine();
                        while ((line = sr.ReadLine()) != null)
                        {
                            //Debug.WriteLine(line);
                        }
                    }
                }

                this.CanRun = true;
                this.CanSave = true;

                this.ProcessWriteLineEvent(this, this.ProjectName + ": loaded");
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.ToString());
            }

        }

        public void RunProject()
        {
            if (!this.CanRun)
                return;
            
            this.SaveProject();
            ProjectResult result = new ProjectResult(this.MainClass.Content, this.MainClass.FileName);
            
            OutputWriteLineEvent(this, ">------ Program started ------<");
            ErrorsWriteLineEvent(this, ">------ Program started ------<");
            
            var startTime = DateTime.Now;
            ProcessWriteLineEvent(this, "> Run project " + this.ProjectName);

            System.Threading.Thread t = new System.Threading.Thread(delegate()
            {
                result.StartRunning();
            });
            t.Start();
            t.Join();

            if (result.NoErrors)
            {
                var finishTime = DateTime.Now;
                var time = finishTime - startTime;
                //OutputWriteLineEvent(this, result.RunResultValue);
                OutputWriteLineEvent(this, ">------ Program finished successfully ------<");
                ErrorsWriteLineEvent(this, ">------ Program finished successfully ------<");
                ProcessWriteLineEvent(this, "> Program finished. Execute time:  " + time.TotalMilliseconds.ToString() + " msecs");
            } else
            {
                List<AST.Error> errs = result.Errors;
                int numerrs = errs.Count;
                string message;
                if (numerrs > 1) 
                    message = ">------ Program terminated. " + numerrs.ToString() + " errors ------<";
                else
                    message = ">------ Program terminated. 1 error ------<";
                
                OutputWriteLineEvent(this, message);
                ProcessWriteLineEvent(this, message);

                foreach (AST.Error err in errs)
                {
                    ErrorsWriteLineEvent(this, "Line " + err.Position.StartLine + " Error: " + err.ErrorMessage);
                }
                ErrorsWriteLineEvent(this, ">-----");
            }
        }
    }
}
