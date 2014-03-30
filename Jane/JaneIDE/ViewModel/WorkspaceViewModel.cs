using System;
using System.Windows.Input;

namespace JaneIDE.ViewModel
{
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        RelayCommand _closeCommand;
        protected WorkspaceViewModel()
        {
        }
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(param => this.OnRequestClose());

                return _closeCommand;
            }
        }
        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}