using AI.NET.Logger;
using System.Windows;
using System.Diagnostics;
using AI.NET.Resources.Strings;

namespace AI.NET
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Service.AI.LoadTopics();//Initialize
            Service.SystemPrompt.LoadPrompts();//Initialize
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        /// <summary>
        /// Handle unhandled exceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error("Unexpected exception", e.ExceptionObject as Exception);
#if !DEBUG
            if (MessageBox.Show(
                Strings.ExUnexpected,"AI.NET", MessageBoxButton.YesNo
                , MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                Process.Start("explorer.exe", "https://github.com/SamHou0/AI.NET/issues/new/choose");
            }
            Process.Start("explorer.exe", AppDomain.CurrentDomain.BaseDirectory + "logs");
            Environment.Exit(1);
#endif
        }
    }

}
