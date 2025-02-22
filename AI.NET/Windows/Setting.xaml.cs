using AI.NET.File;

namespace AI.NET.Windows
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : HandyControl.Controls.Window
    {
        public Setting()
        {
            InitializeComponent();
            Settings.SaveSettings(Settings.GetCurrentSettings());
            mem0GroupBox.DataContext = Service.AI.Mem0;
            opanAIGroupBox.DataContext = Service.AI.OpenAI;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.SaveSettings(Settings.GetCurrentSettings());
        }
    }
}
