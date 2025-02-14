using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenAI.Chat;
using Microsoft.VisualBasic;
using OpenAI;
using System.ClientModel;
using MdXaml;
using System.Diagnostics;
using AI.NET.Network.AI;
using AI.NET.Service;
using HandyControl.Controls;
using AI.NET.File;
using AI.NET.Windows;

namespace AI.NET.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        private readonly string markdownNewLine = Environment.NewLine + Environment.NewLine;
        public MainWindow()
        {
            InitializeComponent();
            loadingCircle.Visibility = Visibility.Hidden;
            Settings.ApplySettings(Settings.ReadSettings());
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userInputBox.Text))
                return;

            SetBusyState(true);

            outputBox.Markdown += "User:" + userInputBox.Text + markdownNewLine;
            outputBox.Markdown += "AI:";
            await Service.AI.RequestAIAsync(userInputBox.Text, outputBox);
            outputBox.Markdown += markdownNewLine;

            SetBusyState(false);
        }
        #region AI



        #endregion
        private async void Window_Initialized(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Set the busy state of the UI
        /// </summary>
        /// <param name="isBusy">The busy state</param>
        private void SetBusyState(bool isBusy)
        {
            loadingCircle.Visibility = isBusy ? Visibility.Visible : Visibility.Hidden;
            sendButton.IsEnabled = !isBusy;
            clearButton.IsEnabled = !isBusy;
            clearContextButton.IsEnabled = !isBusy;
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            userInputBox.Text = string.Empty;
        }

        private void ClearContextButton_Click(object sender, RoutedEventArgs e)
        {
            outputBox.Markdown = string.Empty;
            Service.AI.ResetMessages();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            Setting window = new Setting();
            window.ShowDialog();
            Settings.ApplySettings(Settings.ReadSettings());
            Growl.Success("Settings saved and applied!");
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            About window = new About();
            window.ShowDialog();
        }
    }
}