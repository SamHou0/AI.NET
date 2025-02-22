using AI.NET.File;
using AI.NET.Logger;
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
            topicBox.DataContext = Service.AI.Topics;
            promptList.DataContext = Service.SystemPrompt.systemPrompts;
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userInputBox.Text))
                return;
            SetBusyState(true);
            outputBox.Markdown += "User:" + userInputBox.Text + markdownNewLine;
            outputBox.Markdown += "AI:";
            string input = userInputBox.Text;
            userInputBox.Text = string.Empty;
            // Request AI
            try
            { await Service.AI.RequestAIAsync(input, outputBox); }
            catch (Exception ex)
            {
                Growl.Error(new() { StaysOpen = true, Message = ex.Message });
                Log.Error("Error when requesting AI", ex);
            }

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
            newButton.IsEnabled = !isBusy;
            deleteChatButton.IsEnabled = !isBusy;
        }
        private void NewChatButton_Click(object sender, RoutedEventArgs e)
        {
            userInputBox.Text = string.Empty;
            if (promptList.SelectedIndex >= 0)
                Service.AI.NewChat((Data.SystemPrompt)promptList.SelectedItem);
            else
                Growl.Error("No sys prompt selected. Go to sys prompt and set one, then choose it in the ListBox.");
            topicBox.SelectedIndex = Service.AI.Topics.TopicList.Count - 1;
        }

        private void DeleteChatButton_Click(object sender, RoutedEventArgs e)
        {
            outputBox.Markdown = string.Empty;
            int index = topicBox.SelectedIndex;
            Service.AI.DeleteMessages();
            topicBox.SelectedIndex = index - 1;
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            Setting window = new Setting();
            window.ShowDialog();
            Settings.ApplySettings(Settings.ReadSettings());
            Growl.Success("Settings saved and applied!");
            Log.Info("Settings saved and applied");
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            About window = new About();
            window.ShowDialog();
        }

        private async void TopicBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Ensure selection
            if (topicBox.SelectedIndex < 0)
            {
                topicBox.SelectedIndex = 0;
                return;
            }
            SetBusyState(true);
            // Change topic
            outputBox.Markdown = await Service.AI.Topics.CurrentTopic.GetMarkdownAsync();
            SetBusyState(false);
        }

        private void SystemPromptButton_Click(object sender, RoutedEventArgs e)
        {
            SystemPrompt window = new SystemPrompt();
            window.ShowDialog();
        }
    }
}