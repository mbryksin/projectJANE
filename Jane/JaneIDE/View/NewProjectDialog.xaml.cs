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
    /// Interaction logic for NewProjectDialog.xaml
    /// </summary>
    public partial class NewProjectDialog : Window
    {
        public NewProjectDialog()
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
            NewProjectViewModel viewModel = (NewProjectViewModel) this.DataContext;
            viewModel.MainClass = e.Source.ToString();

        }
    }
}
