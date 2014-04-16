using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Globalization;

using JaneIDE.Model;
using JaneIDE.View;
using JaneIDE.ViewModel;

namespace JaneIDE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            // This code is used to test the app when using other cultures.
            //
            //System.Threading.Thread.CurrentThread.CurrentCulture =
            //    System.Threading.Thread.CurrentThread.CurrentUICulture =
            //        new System.Globalization.CultureInfo("it-IT");


            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            //
            FrameworkElement.LanguageProperty.OverrideMetadata(
              typeof(FrameworkElement),
              new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        static void c_NewProject(object sender, EventArgs e)
        {
            NewProjectDialog newProjectDialogWindow = new NewProjectDialog();
            NewProjectEventArgs args = (NewProjectEventArgs)e;
            var viewModel = new NewProjectViewModel(args.Project);

            EventHandler handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                newProjectDialogWindow.Close();
            };
            viewModel.RequestClose += handler;

            newProjectDialogWindow.DataContext = viewModel;
            newProjectDialogWindow.ShowDialog();
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow window = new MainWindow();

            var viewModel = new MainWindowViewModel();

            // When the ViewModel asks to be closed, 
            // close the window.
            EventHandler handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                window.Close();
            };
            viewModel.RequestClose += handler;
            viewModel.NewProjectEvent += c_NewProject;

            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree.
            window.DataContext = viewModel;

            window.Show();
        }
    }
}