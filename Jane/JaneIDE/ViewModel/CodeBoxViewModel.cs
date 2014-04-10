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
        public CodeBoxViewModel(string displayName)
        {
            base.DisplayName = displayName;
        }
    }
}
