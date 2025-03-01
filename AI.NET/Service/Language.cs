using AI.NET.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AI.NET.Service
{
    class Language
    {
        public static void ChangeLanguage(string cultureName)
        {
            CultureInfo culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow mainWindow)
                {
                    mainWindow.Resources.MergedDictionaries.Clear();
                    mainWindow.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/LangResource.xaml", UriKind.Relative) }); //重新加载
                }
            }
        }
    }
}
