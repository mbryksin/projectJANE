using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

using System.IO;
using System.Windows.Forms;

using JaneIDE.Model;
using JaneIDE.View;

namespace JaneIDE.ViewModel
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        Project project;        
        ObservableCollection<WorkspaceViewModel> _workspaces;
        RelayCommand _newProjectCommand;
        RelayCommand _openProjectCommand;
        private OpenFileDialog openFileDialog;
        private string output;
        private string errors;
        private string process;

        public MainWindowViewModel()
        {
            project = new Project();
            project.OutputWriteLineEvent += OutputWriteLine;
            project.OutputWriteEvent += OutputWrite;
            project.ErrorsWriteLineEvent += ErrorsWriteLine;
            project.ProcessWriteLineEvent += ProcessWriteLine;

            openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.SpecialFolder.Personal.ToString();
            openFileDialog.Filter = "Project file (*.pro)|*.pro";
            openFileDialog.RestoreDirectory = true;
        }

        void OutputWriteLine(object sender, String e)
        {
            this.OutputText = this.OutputText + e + "\r";
        }
        void OutputWrite(object sender, String e)
        {
            this.OutputText = this.OutputText + e;
        }
        void ErrorsWriteLine(object sender, String e)
        {
            this.ErrorsText = this.ErrorsText + e + "\r";
        }
        void ProcessWriteLine(object sender, String e)
        {
            this.ProcessText = this.ProcessText + e + "\r";
        }

        public string OutputText
        {
            get { return output; }
            set
            {
                if (value == output)
                    return;
                output = value;
                base.OnPropertyChanged("OutputText");
            }
        }
        public string ErrorsText
        {
            get { return errors; }
            set
            {
                if (value == errors)
                    return;
                errors = value;
                base.OnPropertyChanged("ErrorsText");
            }
        }
        public string ProcessText
        {
            get { return process; }
            set
            {
                if (value == process)
                    return;
                process = value;
                base.OnPropertyChanged("ProcessText");
            }
        }
        public ICommand OpenProjectCommand
        {
            get
            {
                if (_openProjectCommand == null)
                    _openProjectCommand = new RelayCommand(param => this.OpenProject(),
                                                           param => !project.CanStop);

                return _openProjectCommand;
            }
        }
        public ICommand SaveProjectCommand
        {
            get { return new RelayCommand( param => project.SaveProject(), param => (project.CanSave & !project.CanStop) ); }
        }
        public ICommand RunProjectCommand
        {
            get { return new RelayCommand(param => project.RunProject(), param => project.CanRun); }
        }
        public ICommand StopProjectCommand
        {
            get { return new RelayCommand(param => project.StopRunning(), param => project.CanStop); }
        }
        public ICommand NewFileCommand
        {
            get { return new RelayCommand(param => this.OnRequestNewFileDialog(), param => !project.CanStop); }
        }
        public event EventHandler NewFileEvent;
        public void OnRequestNewFileDialog()
        {
            EventHandler handler = this.NewFileEvent;
            if (handler != null)
                handler(this, new EventArgs());
        }
        public ICommand NewProjectCommand
        {
            get
            {
                if (_newProjectCommand == null)
                    _newProjectCommand = new RelayCommand(param => this.OnRequestNewProject(),
                                                          param => !project.CanStop);

                return _newProjectCommand;
            }
        }
        public event EventHandler NewProjectEvent;
        void OnRequestNewProject()
        {
            EventHandler handler = this.NewProjectEvent;
            if (handler != null)
                handler(this, new NewProjectEventArgs(project));
        }

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }

                return _workspaces;
            }
        }
        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }
        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }
        public void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            //Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        public void CreateNewFile(string filename)
        {
            Source src = new Source(filename);
            project.AddSource(src);
            CodeBoxViewModel workspace = new CodeBoxViewModel(src);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        public void CreateMainClassWorkspace()
        {
            Workspaces.Clear();
            CodeBoxViewModel workspace = new CodeBoxViewModel(project.MainClass);
            
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void OpenProject()
        {
            string path = String.Empty;
            Stream myStream = null;
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            using (StreamReader reader = new StreamReader(myStream))
                            {
                                path = reader.ReadLine();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
                string filePath = openFileDialog.FileName;
                string folderPath = Path.GetDirectoryName(filePath);
                project.OpenProject(filePath, folderPath);
                this.Workspaces.Clear();
                CodeBoxViewModel workspace = new CodeBoxViewModel(project.MainClass);
                this.Workspaces.Add(workspace);
                this.SetActiveWorkspace(workspace);
            }
        }

    }
}