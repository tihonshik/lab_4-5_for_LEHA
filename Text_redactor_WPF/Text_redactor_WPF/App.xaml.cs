﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace Text_redactor_WPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static List<CultureInfo> m_Languages = new List<CultureInfo>();
        public static List<CultureInfo> Languages
        {
            get
            {
                return m_Languages;
            }
        }
        public static event EventHandler LanguageChanged;

        void App_LanguageChanged(Object sender, EventArgs e)
        {
            Text_redactor_WPF.Properties.Settings.Default.DefaultLanguage = Language;
            Text_redactor_WPF.Properties.Settings.Default.Save();
        }

        public App()
        {
            InitializeComponent();
            App.LanguageChanged += App_LanguageChanged;

            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US"));
            m_Languages.Add(new CultureInfo("ru-RU"));

            Language = Text_redactor_WPF.Properties.Settings.Default.DefaultLanguage;
        }




        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;


                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                        dict.Source = new Uri(String.Format("lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("lang.xaml", UriKind.Relative);
                        break;
                }


                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("lang.")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }


                LanguageChanged(Application.Current, new EventArgs());

            }
        }
    }
}
