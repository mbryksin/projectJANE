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

using JaneIDE.View;

namespace JaneIDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //CodeBoxConf();
        }
        /*
        public void CodeBoxConf()
        {
            ScintillaNET.Scintilla CodeBox = (ScintillaNET.Scintilla)wfh.Child;

            CodeBox.ConfigurationManager.Language = "jane";
            CodeBox.ConfigurationManager.CustomLocation = @"JaNE.xml";
            CodeBox.ConfigurationManager.Configure();

            CodeBox.Margins[0].Width = 25; // String numbers
            CodeBox.Margins[2].Width = 20;

            CodeBox.CharAdded += CodeBox_CharAdded;
            CodeBox.AutoComplete.MaxWidth = 20;
            CodeBox.AutoComplete.MaxHeight = 10;
            CodeBox.AutoComplete.List.Sort();

            CodeBox.Caret.HighlightCurrentLine = true;
            CodeBox.Caret.CurrentLineBackgroundColor = System.Drawing.Color.FromArgb(245, 245, 245);

            CodeBox.Folding.IsEnabled = true;
            CodeBox.Folding.UseCompactFolding = true;

            CodeBox.Selection.BackColor = System.Drawing.Color.FromArgb(190, 190, 190);

            CodeBox.IsBraceMatching = true;

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

        private void wfh_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
        */
    }
}
