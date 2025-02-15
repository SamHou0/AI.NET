﻿using AI.NET.Logger;
using System.Diagnostics;
using System.Windows;

namespace AI.NET
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
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
            if(MessageBox.Show(
                "An unexpected error occurred. Calm down, it's not your fault. We've saved the information to the logs subfolder in your app folder, please create a new bug report issue on GitHub with the logs. Do you want to go there now?",
                "AI.NET", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                Process.Start("explorer.exe", "https://github.com/SamHou0/AI.NET/issues/new/choose");
            }
            Process.Start("explorer.exe", AppDomain.CurrentDomain.BaseDirectory + "logs");
            Environment.Exit(1);
        }
    }

}
