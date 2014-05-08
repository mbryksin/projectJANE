using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaneIDE.Model
{
    class Source
    {
        private string fileName;
        private string content;

        public Source(string sourceName)
        {
            this.FileName = sourceName;
            this.content = String.Empty;
        }
        
        public string FileName
        {
            get { return fileName; }
            set 
            {
                if (value == fileName)
                    return;

                fileName = value;
            }
        }

        public string Content
        {
            get { return content; }
            set
            {
                if (value.Trim() == content)
                    return;

                content = value.Trim();
            }
        }
    }
}
