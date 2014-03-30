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
        RelayCommand _newProjectCommand;

        public MainWindowViewModel()
        {
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
    }
}