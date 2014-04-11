using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaneIDE.Model
{
    class NewProjectEventArgs : EventArgs
    {
        public NewProjectEventArgs(Project newProject)
        {
            this.Project = newProject;
        }

        public Project Project { get; private set; }
    }
}
