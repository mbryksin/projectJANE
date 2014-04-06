using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace JaneIDE.ViewModel
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        ObservableCollection<WorkspaceViewModel> _workspaces;
        RelayCommand _newProjectCommand;

        public MainWindowViewModel()
        {
            //CustomerViewModel workspace = new CustomerViewModel(newCustomer, _customerRepository);
            //this.Workspaces.Add(workspace);
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
                handler(this, EventArgs.Empty);
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
    }
}