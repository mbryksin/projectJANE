using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JaneIDE.ViewModel
{
    class NewFileViewModel : WorkspaceViewModel
    {
        private string fileName;
        RelayCommand _okCommand;
        bool finished;

        public NewFileViewModel()
        {
            fileName = String.Empty;
            finished = false;            
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                if (value.Trim() == fileName)
                    return;
                fileName = value.Trim();

                base.OnPropertyChanged("FileName");
            }
        }

        override public string DisplayName
        {
            get { return "New File Dialog"; }
        }

        public ICommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    _okCommand = new RelayCommand(
                        param => this.Finish(),
                        param => this.CanFinish
                        );
                }
                return _okCommand;
            }
        }


        public bool CanFinish
        {
            get { return !String.IsNullOrEmpty(this.FileName); }
        }

        public bool Finished { get { return finished; } }

        public void Finish()
        {
            if (this.CanFinish)
            {
                finished = true;
                this.CloseCommand.Execute(null);
            }
        }
    }
}
