namespace AI.NET.Windows
{
    /// <summary>
    /// SystemPrompt.xaml 的交互逻辑
    /// </summary>
    public partial class SystemPrompt : HandyControl.Controls.Window
    {
        public SystemPrompt()
        {
            InitializeComponent();
            promptDataGrid.DataContext =
                Service.SystemPrompt.systemPrompts;
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Service.SystemPrompt.AddPrompt(new());
        }
        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Service.SystemPrompt.RemovePrompt(
                (Data.SystemPrompt)promptDataGrid.SelectedItem);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Service.SystemPrompt.Save();
        }
    }
}
