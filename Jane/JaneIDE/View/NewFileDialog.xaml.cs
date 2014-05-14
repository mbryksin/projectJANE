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
using System.Windows.Shapes;

using JaneIDE.ViewModel;

namespace JaneIDE.View
{
    /// <summary>
    /// Interaction logic for NewFileDialog.xaml
    /// </summary>
    public partial class NewFileDialog : Window
    {
        public NewFileDialog()
        {
            InitializeComponent();
        }
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MinHeight = this.ActualHeight;
            this.MaxHeight = this.ActualHeight;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = (NewFileViewModel)this.DataContext;
            var textbox = sender as TextBox;
            string filename = textbox.Text;
            viewModel.FileName = filename;
        }
    }
}
