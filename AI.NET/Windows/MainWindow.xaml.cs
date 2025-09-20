using AI.NET.Data;
using AI.NET.File;
using AI.NET.Logger;
using AI.NET.Resources.Strings;
using HandyControl.Controls;
using System.Windows;
using System.Windows.Input;

namespace AI.NET.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        private readonly string markdownNewLine = Environment.NewLine + Environment.NewLine;
        private bool isClosable = false;
        public MainWindow()
        {
            InitializeComponent();
            topicBox.DataContext = Service.AI.Topics;
            promptList.DataContext = Service.SystemPrompt.systemPrompts;
            //Apply user window position
            try
            {
                Rect restoreBounds = Properties.Settings.Default.MainRestoreBounds;
                WindowState = WindowState.Normal;
                Left = restoreBounds.Left;
                Top = restoreBounds.Top;
                Width = restoreBounds.Width;
                Height = restoreBounds.Height;
                WindowState = Properties.Settings.Default.MainWindowState;
            }
            catch (Exception ex)
            {
                Log.Error(Strings.ExWindowPosition, ex);
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userInputBox.Text))
            {
                SetBusyState(false);//Avoid toggle button error
                return;
            }

            SetBusyState(true);
            string input = userInputBox.Text;
            userInputBox.Text = string.Empty;
            // Request AI
            try
            {
                if (topicBox.Items.Count <= 0)
                {
                    Growl.Error(new() { StaysOpen = true, Message = Strings.ExNoTopicSelection });
                    userInputBox.Text = input;//Recover input
                    return;
                }
                await Service.AI.RequestAIAsync(input, outputBox);
            }
            catch (Exception ex)
            {
                Growl.Error(new() { StaysOpen = true, Message = ex.Message });
                Log.Error("Error when requesting AI", ex);
            }
            finally
            {
                outputBox.Markdown += markdownNewLine;
                SetBusyState(false);
            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            if (Settings.SettingsExist)
                Settings.ApplySettings(Settings.ReadSettings());
            else
                Growl.Warning(new()
                { StaysOpen = true, Message = Strings.ExAINotSet });
        }
        /// <summary>
        /// Set the busy state of the UI
        /// </summary>
        /// <param name="isBusy">The busy state</param>
        private void SetBusyState(bool isBusy)
        {
            sendButton.IsChecked = isBusy;
            sendButton.IsEnabled = !isBusy;
            newButton.IsEnabled = !isBusy;
            retryButton.IsEnabled = !isBusy;
        }
        private void NewChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (promptList.SelectedIndex >= 0)
                Service.AI.NewChat((Data.SystemPrompt)promptList.SelectedItem);
            else
                Growl.Error(Strings.ExNoSysPromptSelection);
            topicBox.SelectedIndex = Service.AI.Topics.TopicList.Count - 1;
        }

        private async void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            if (topicBox.SelectedIndex >= 0 && topicBox.Items.Count > 0)
            {
                SetBusyState(true);
                try
                {
                    await Service.AI.RetryLastResponseAsync(outputBox);
                }
                catch (Exception ex)
                {
                    Growl.Error(new() { StaysOpen = true, Message = ex.Message });
                    Log.Error("Error when retrying AI response", ex);
                }
                finally
                {
                    outputBox.Markdown += markdownNewLine;
                    SetBusyState(false);
                }
            }
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            Setting window = new Setting();
            window.ShowDialog();
            Settings.ApplySettings(Settings.ReadSettings());
            Growl.Success(Strings.InfoSettingSaved);
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
            if (topicBox.SelectedIndex >= 0 && topicBox.Items.Count > 0)
            {
                SetBusyState(true);
                // Change topic
                outputBox.Markdown = await Service.AI.Topics.CurrentTopic.GetMarkdownAsync();
                SetBusyState(false);
            }
        }

        private void SystemPromptButton_Click(object sender, RoutedEventArgs e)
        {
            SystemPrompt window = new SystemPrompt();
            window.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isClosable)
            {
                Properties.Settings.Default.MainRestoreBounds = RestoreBounds;
                Properties.Settings.Default.MainWindowState = WindowState;
                Properties.Settings.Default.Save();
            }
            else
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.N)
                    NewChatButton_Click(sender, e);
                else if (e.Key == Key.Enter)
                    SendButton_Click(sender, e);
                else if (e.Key == Key.R)
                    RetryButton_Click(sender, e);
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            isClosable = true;
            Close();
        }

        private void ShowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Show();
        }
    }
}