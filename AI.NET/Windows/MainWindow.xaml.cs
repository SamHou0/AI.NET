using AI.NET.File;
using HandyControl.Controls;
using System.Windows;

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
        private void Window_Activated(object sender, EventArgs e)
        {
            if (Settings.SettingsExist)
                Settings.ApplySettings(Settings.ReadSettings());
            else
                Growl.Warning(new()
                { StaysOpen = true, Message = "Please set your AI in the Settings first!" });
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