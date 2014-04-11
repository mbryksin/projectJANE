using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

using JaneIDE.Model;

namespace JaneIDE.ViewModel
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        Project project;        
        ObservableCollection<WorkspaceViewModel> _workspaces;
        RelayCommand _newProjectCommand;

        public MainWindowViewModel()
        {
            project = null;

            this.CreateNewFile("Program.jane");
        }

        public ICommand NewProjectCommand
        {
            get
            {
                if (_newProjectCommand == null)
                    _newProjectCommand = new RelayCommand(param => this.OnRequestNewProject());

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

        void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            //Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        void CreateNewFile(string filename)
        {
            CodeBoxViewModel workspace = new CodeBoxViewModel(filename);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }
    }
}