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
        private MainWindowViewModel viewModel;

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

        void c_NewProject(object sender, EventArgs e)
        {
            NewProjectDialog newProjectDialogWindow = new NewProjectDialog();
            NewProjectEventArgs args = (NewProjectEventArgs)e;
            var newProjectViewModel = new NewProjectViewModel(args.Project);

            EventHandler handler = null;
            handler = delegate
            {
                newProjectViewModel.RequestClose -= handler;
                newProjectDialogWindow.Close();
                if (newProjectViewModel.Saved)
                    viewModel.CreateMainClassWorkspace();
            };

            newProjectViewModel.RequestClose += handler;

            newProjectDialogWindow.DataContext = newProjectViewModel;
            newProjectDialogWindow.ShowDialog();
            
        }

        void c_NewFile(object sender, EventArgs e)
        {
            NewFileDialog newFileDialogWindow = new NewFileDialog();
            var newFileViewModel = new NewFileViewModel();

            EventHandler handler = null;
            handler = delegate
            {
                newFileViewModel.RequestClose -= handler;
                newFileDialogWindow.Close();
                if (newFileViewModel.Finished)
                    viewModel.CreateNewFile(newFileViewModel.FileName);
            };

            newFileViewModel.RequestClose += handler;

            newFileDialogWindow.DataContext = newFileViewModel;
            newFileDialogWindow.ShowDialog();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow window = new MainWindow();
            viewModel = new MainWindowViewModel();

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
            viewModel.NewFileEvent += c_NewFile;

            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree.
            window.DataContext = viewModel;

            window.Show();
        }
    }
}