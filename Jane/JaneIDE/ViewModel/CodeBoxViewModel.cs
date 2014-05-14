using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JaneIDE.Model;

namespace JaneIDE.ViewModel
{
    class CodeBoxViewModel : WorkspaceViewModel
    {
        private string content;
        private Source src;

        public CodeBoxViewModel(Source source)
        {
            content = String.Empty;
            src = source;
            base.DisplayName = src.FileName;            
        }

        public string CodeboxText
        {
            get { return content; }
            set
            {
                if (value == content)
                    return;
                content = value;
                src.Content = content;
            }
        }
        
        
    }
}
