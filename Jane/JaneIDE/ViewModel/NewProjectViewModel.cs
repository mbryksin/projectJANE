using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;
using System.Threading.Tasks;
using JaneIDE.Model;

namespace JaneIDE.ViewModel
{
    class NewProjectViewModel : WorkspaceViewModel
    {
        private FolderBrowserDialog folderBrowserDialog;

        RelayCommand _browseCommand;
        RelayCommand _saveCommand;

        string folderName;
        string projectName;
        string projectFolder;

        public NewProjectViewModel()
        {
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog.Description = "Select the Jane project directory";
            //this.folderBrowserDialog.ShowNewFolderButton = true;
            // Default to the My Documents folder.
            this.folderBrowserDialog.RootFolder = Environment.SpecialFolder.Personal;
        }

        public string ProjectName
        {
            get { return projectName; }
            set
            {
                if (value == projectName)
                    return;

                projectName = value;

                base.OnPropertyChanged("ProjectName");

                this.ProjectFolder = this.ProjectLocation + "\\" + this.ProjectName;
            }
        }

        public string ProjectFolder
        {
            get { return projectFolder; }
            set
            {
                if (this.ProjectLocation == "")
                    return;
                if (this.ProjectName == "")
                    return;

                projectFolder = value;

                base.OnPropertyChanged("ProjectFolder");

                this.ProjectFolder = this.ProjectLocation + "\\" + this.ProjectName;
            }
        }

        public string ProjectLocation
        {
            get { return folderName; }
            set
            {
                if (value == folderName)
                    return;

                folderName = value;

                base.OnPropertyChanged("ProjectLocation");

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

        bool CanSave
        {
            get { return true; /* String.IsNullOrEmpty(this.ValidateCustomerType()) && _customer.IsValid; */}
        }

        public void Save()
        {
            
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
