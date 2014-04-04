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
            CodeBoxConf();
        }
        public void CodeBoxConf()
        {
            ScintillaNET.Scintilla CodeBox = (ScintillaNET.Scintilla)wfh.Child;
            CodeBox.ConfigurationManager.Language = "jane";
            CodeBox.ConfigurationManager.CustomLocation = @"JaNE.xml";
            CodeBox.ConfigurationManager.Configure();
            CodeBox.Margins[0].Width = 25; // String numbers
            //CodeBox.BackColor 
            CodeBox.CharAdded += CodeBox_CharAdded;
            CodeBox.AutoComplete.MaxWidth = 20;
            CodeBox.AutoComplete.MaxHeight = 10;
        }

        void CodeBox_CharAdded(object sender, ScintillaNET.CharAddedEventArgs e)
        {
            ((ScintillaNET.Scintilla)wfh.Child).AutoComplete.Show();
           //throw new NotImplementedException();
        }

        private void wfh_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
