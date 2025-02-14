﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.Net.Http;
using HandyControl.Controls;

namespace AI.NET.Windows
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : HandyControl.Controls.Window
    {
        private readonly string url =
            "https://github.com/SamHou0/AI.NET/raw/refs/heads/main/README.md";
        private readonly string filePath =
            AppDomain.CurrentDomain.BaseDirectory + "README.md";
        public About()
        {
            InitializeComponent();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            aboutBox.Markdown = "*Loading...*";
            if (System.IO.File.Exists(filePath))
            {
                using (StreamReader reader = new(filePath))
                {
                    aboutBox.Markdown = await reader.ReadToEndAsync();
                }
            }
            using (HttpClient client = new())
            {
                Growl.Info("Updating README.md...");
                var response = await client.GetAsync(url);
                using (StreamWriter writer = new(filePath))
                {
                    writer.Write(await response.Content.ReadAsStringAsync());
                }
            }
            using (StreamReader reader = new(filePath))
            {
                aboutBox.Markdown = await reader.ReadToEndAsync();
                Growl.Success("Successfully update README.md");
            }
        }
    }
}
