using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Input;
using System.Windows.Forms;
using System.Threading.Tasks;

using JaneIDE.Model;

namespace JaneIDE.ViewModel
{
    class NewProjectViewModel : WorkspaceViewModel
    {
        private FolderBrowserDialog folderBrowserDialog;
        Project newProject;
        RelayCommand _browseCommand;
        RelayCommand _saveCommand;

        string folderName;
        string projectName;
        string projectFolder;
        string mainClass;

        public NewProjectViewModel(Project project)
        {
            newProject = project;
            projectFolder = "";
            projectName = "";
            folderName = "";
            mainClass = "";
            this.folderBrowserDialog = new FolderBrowserDialog();
            this.folderBrowserDialog.Description = "Select the Jane project directory";
            this.folderBrowserDialog.RootFolder = Environment.SpecialFolder.Personal;
        }

        public string MainClass
        {
            get { return mainClass; }
            set
            {
                if (value.Trim() == mainClass)
                    return;
                mainClass = value.Trim();

                base.OnPropertyChanged("MainClass");
            }
        }

        public string ProjectName
        {
            get { return projectName; }
            set
            {
                if (value.Trim() == projectName)
                    return;

                projectName = value.Trim();

                base.OnPropertyChanged("ProjectName");

                this.ProjectFolder = System.IO.Path.Combine(this.ProjectLocation, projectName);
            }
        }
        public string ProjectLocation
        {
            get { return folderName; }
            set
            {
                if (value.Trim() == folderName)
                    return;

                /*
                if (!Directory.Exists(value.Trim()))
                {
                    folderName = "";
                    return;
                }
                */

                folderName = value.Trim();

                base.OnPropertyChanged("ProjectLocation");

                this.ProjectFolder = System.IO.Path.Combine(folderName, this.ProjectName);
            }
        }
        public string ProjectFolder
        {
            get { return projectFolder; }
            set
            {
                if (this.ProjectLocation == "" || this.ProjectName == "")
                {
                    if (projectFolder == "")
                        return;

                    projectFolder = "";
                    base.OnPropertyChanged("ProjectFolder");
                    return;
                }

                if (value.Trim() == projectFolder)
                    return;

                projectFolder = value.Trim();

                base.OnPropertyChanged("ProjectFolder");
            }
        }

        override public string DisplayName
        {
            get { return "New Jane Project"; }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => this.Save(),
                        param => this.CanSave
                        );
                }
                return _saveCommand;
            }
        }

        public ICommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                {
                    _browseCommand = new RelayCommand(
                        param => this.Browse(),
                        param => true
                        );
                }
                return _browseCommand;
            }
        }

        public bool CanSave
        {
            get { return !(String.IsNullOrEmpty(this.ProjectFolder) || String.IsNullOrEmpty(this.ProjectName) || String.IsNullOrEmpty(this.MainClass)); }
        }

        public void Save()
        {
            if (this.CanSave)
            {
                newProject = new Project(this.ProjectName, this.ProjectFolder, this.MainClass);
                newProject.SaveProject();
                this.CloseCommand.Execute(null);
            }
        }

        public void Browse()
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.ProjectLocation = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
