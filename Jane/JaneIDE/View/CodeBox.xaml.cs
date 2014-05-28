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
        public ScintillaNET.Scintilla codeBox; 
        public CodeBox()
        {
            InitializeComponent();

            // Initialize CodeBox by ScintillaNET

            // Integration fix from official instruction
            codeBox = (ScintillaNET.Scintilla)wfh.Child;

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(Reload);

            // codeBox configuration from JaNE.xml
            codeBox.ConfigurationManager.Language = "jane";
            codeBox.ConfigurationManager.CustomLocation = @"JaNE.xml";
            codeBox.ConfigurationManager.Configure();

            // codeBox's margins configuration for string numbers and code folding
            codeBox.Margins[0].Width = 25; 
            codeBox.Margins[2].Width = 20;

            // codeBox's autocomplete
            codeBox.CharAdded += codeBox_CharAdded;
            codeBox.AutoComplete.MaxWidth = 20;
            codeBox.AutoComplete.MaxHeight = 10;
            codeBox.AutoComplete.List.Sort();

            // Highlighting current line (light grey)
            codeBox.Caret.HighlightCurrentLine = true;
            codeBox.Caret.CurrentLineBackgroundColor = System.Drawing.Color.FromArgb(245, 245, 245);

            //Code folding from default lexer
            codeBox.Folding.IsEnabled = true;
            codeBox.Folding.UseCompactFolding = true;

            //Background color
            codeBox.Selection.BackColor = System.Drawing.Color.FromArgb(190, 190, 190);

            //Brace Matching (red and bold)
            codeBox.IsBraceMatching = true;

            //Handler for snippets
            codeBox.KeyDown += codeBox_KeyDown;
        }

        private void Reload(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext == null)
                return;
            var viewModel = (CodeBoxViewModel)this.DataContext; 
            codeBox.Text = viewModel.CodeboxText;
        }

        void codeBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == System.Windows.Forms.Keys.Space && e.Control)
            {
                codeBox.Snippets.ShowSnippetList();
            }
        }

        void codeBox_CharAdded(object sender, ScintillaNET.CharAddedEventArgs e)
        {
           
            if (Char.IsLetter(e.Ch))
            codeBox.AutoComplete.Show();
        }

        private void codeBox_TextChanged(object sender, EventArgs e)
        {
            var viewModel = (CodeBoxViewModel)this.DataContext;
            ScintillaNET.Scintilla CodeBox = sender as ScintillaNET.Scintilla;
            string codeboxtext = codeBox.Text;
            viewModel.CodeboxText = codeboxtext;
            //TODO: BALLOON with ' '
        }

        void ErrorMarker(int stringNum)
        {
            Marker marker = codeBox.Markers[0];
            marker.Symbol = MarkerSymbol.Circle;
            marker.BackColor = System.Drawing.Color.Red;
            codeBox.Lines[stringNum - 1].AddMarker(marker);
        }
        void BreakPointMarker(int stringNum)
        {
            Marker marker = codeBox.Markers[1];
            marker.Symbol = MarkerSymbol.FullRectangle;
            marker.BackColor = System.Drawing.Color.Blue;
            codeBox.Lines[stringNum - 1].AddMarker(marker);
        }
        void CurrentMarker(int stringNum)
        {
            Marker marker = codeBox.Markers[3];
            marker.Symbol = MarkerSymbol.Arrow;
            marker.BackColor = System.Drawing.Color.Green;
            codeBox.Lines[stringNum - 1].AddMarker(marker);
        }

        void AddContext(List<string> context)
        {
            codeBox.AutoComplete.List.AddRange(context);
            codeBox.AutoComplete.List = codeBox.AutoComplete.List.Distinct().ToList();
            codeBox.AutoComplete.List.Sort();
        }
        void RemoveContext(List<string> context)
        {
            codeBox.AutoComplete.List = codeBox.AutoComplete.List.Except(context).ToList();
            codeBox.AutoComplete.List.Sort();
        } 
    }
}
