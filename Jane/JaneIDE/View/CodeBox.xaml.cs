using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScintillaNET;
using JaneIDE.ViewModel;

namespace JaneIDE.View
{
    /// <summary>
    /// Interaction logic for CodeBox.xaml
    /// </summary>
    public partial class CodeBox : UserControl
    {
        public CodeBox()
        {
            InitializeComponent();

            // Initialize CodeBox by ScintillaNET

            // Integration fix from official instruction
            ScintillaNET.Scintilla CodeBox = (ScintillaNET.Scintilla)wfh.Child; 

            // Codebox configuration from JaNE.xml
            CodeBox.ConfigurationManager.Language = "jane";
            CodeBox.ConfigurationManager.CustomLocation = @"JaNE.xml";
            CodeBox.ConfigurationManager.Configure();

            // Codebox's margins configuration for string numbers and code folding
            CodeBox.Margins[0].Width = 25; 
            CodeBox.Margins[2].Width = 20;

            // CodeBox's autocomplete
            CodeBox.CharAdded += CodeBox_CharAdded;
            CodeBox.AutoComplete.MaxWidth = 20;
            CodeBox.AutoComplete.MaxHeight = 10;
            CodeBox.AutoComplete.List.Sort();

            // Highlighting current line (light grey)
            CodeBox.Caret.HighlightCurrentLine = true;
            CodeBox.Caret.CurrentLineBackgroundColor = System.Drawing.Color.FromArgb(245, 245, 245);

            //Code folding from default lexer
            CodeBox.Folding.IsEnabled = true;
            CodeBox.Folding.UseCompactFolding = true;

            //Background color
            CodeBox.Selection.BackColor = System.Drawing.Color.FromArgb(190, 190, 190);

            //Brace Matching (red and bold)
            CodeBox.IsBraceMatching = true;

            //Handler for snippets
            CodeBox.KeyDown += CodeBox_KeyDown;

            
        }


        void CodeBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == System.Windows.Forms.Keys.Space && e.Control)
            {
                ((ScintillaNET.Scintilla)wfh.Child).Snippets.ShowSnippetList();
            }
        }

        void CodeBox_CharAdded(object sender, ScintillaNET.CharAddedEventArgs e)
        {
           
            if (Char.IsLetter(e.Ch))
            ((ScintillaNET.Scintilla)wfh.Child).AutoComplete.Show();
        }

        private void CodeBox_TextChanged(object sender, EventArgs e)
        {
            var viewModel = (CodeBoxViewModel)this.DataContext;
            ScintillaNET.Scintilla CodeBox = sender as ScintillaNET.Scintilla;
            string codeboxtext = CodeBox.Text;
            viewModel.CodeboxText = codeboxtext;
            //TODO: BALLOON with ' '
        }

    
    }
}
