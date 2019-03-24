using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;



namespace Text_redactor_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TabItem> tabItems = new List<TabItem>();
        public MainWindow()
        {
            InitializeComponent();
            
            _Font.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            // создаем привязку команды
            CommandBinding commandBinding = new CommandBinding();
            // устанавливаем команду
            commandBinding.Command = ApplicationCommands.Help;
            // устанавливаем метод, который будет выполняться при вызове команды
            commandBinding.Executed += CommandBinding_Executed;
            // добавляем привязку к коллекции привязок элемента Button
            _Help.CommandBindings.Add(commandBinding);

            App.LanguageChanged += LanguageChanged;

            CultureInfo currLang = App.Language;


            menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                MenuItem menuLang = new MenuItem();
                menuLang.Header = lang.DisplayName;
                menuLang.Tag = lang;
                menuLang.IsChecked = lang.Equals(currLang);
                menuLang.Click += ChangeLanguageClick;
                menuLanguage.Items.Add(menuLang);
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("TextEditor PRO v2.1", "Help");
        }

        private void FontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_Font.SelectedItem != null)
            {
                TabItem tab = _ControlBox.SelectedItem as TabItem;
                RichTextBox RtextBox = tab.Content as RichTextBox;
                RtextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, _Font.SelectedItem);
            }
        }

        private void _NewFile_Click(object sender, RoutedEventArgs e)
        {

            int index = TabItemManipulate.GetIndex;

            RichTextBox richTextBox = RichTExtBoxCreatetor.CreateRichTextBox(item_Click_Close,
                RichTextBox_DragOver,
                RichTextBox_Drop,
                RichTextBox_TextChanged
            );
            TabItemManipulate.NewTabItem("Новый файл " + index, richTextBox, _ControlBox);

        }

        #region context Menu готово
        public void item_Click_Close(Object sender, RoutedEventArgs e)
        {
            _ControlBox.Items.RemoveAt(_ControlBox.SelectedIndex);
        }
        #endregion

        #region drag and drop готово
        private void RichTextBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = false;
        }

        private void RichTextBox_Drop(object sender, DragEventArgs e)
        {

            RichTextBox currentRichTextBox = TabItemManipulate.GetCurrentRichTextBox(_ControlBox);

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] docPath = (string[])e.Data.GetData(DataFormats.FileDrop);

                var dataFormat = DataFormats.Text;


                System.Windows.Documents.TextRange range;
                System.IO.FileStream fStream;
                if (System.IO.File.Exists(docPath[0]))
                {
                    try
                    {
                        // Open the document in the RichTextBox.
                        range = new System.Windows.Documents.TextRange(currentRichTextBox.Document.ContentStart, currentRichTextBox.Document.ContentEnd);
                        fStream = new System.IO.FileStream(docPath[0], System.IO.FileMode.OpenOrCreate);
                        range.Load(fStream, dataFormat);
                        fStream.Close();
                    }
                    catch (System.Exception)
                    {
                        //MessageBox.Show("File could not be opened. Make sure the file is a text file.");
                    }
                }
            }
        }
        #endregion



        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            RichTextBox Vova = sender as RichTextBox;
            TextRange VovaCount = new TextRange(Vova.Document.ContentStart, Vova.Document.ContentEnd);
            CountWord.Content = "Count word: " + (VovaCount.Text.Length - 2);
        }


        private void _FontSize_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_ControlBox != null)
            {
                TabItem tab = _ControlBox.SelectedItem as TabItem;
                RichTextBox RtextBox = tab.Content as RichTextBox;
                RtextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, _FontSize.Value);
            }
        }

        private void _Black_Click(object sender, RoutedEventArgs e)
        {
            if (_ControlBox != null)
            {
                TabItem tab = _ControlBox.SelectedItem as TabItem;
                RichTextBox RtextBox = tab.Content as RichTextBox;
                RtextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, Brushes.Black);
            }
        }

        private void _Blue_Click(object sender, RoutedEventArgs e)
        {
            if (_ControlBox != null)
            {
                TabItem tab = _ControlBox.SelectedItem as TabItem;
                RichTextBox RtextBox = tab.Content as RichTextBox;
                RtextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, Brushes.Blue);
            }
        }

        private void _Red_Click(object sender, RoutedEventArgs e)
        {
            if (_ControlBox != null)
            {
                TabItem tab = _ControlBox.SelectedItem as TabItem;
                RichTextBox RtextBox = tab.Content as RichTextBox;
                RtextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, Brushes.Red);
            }
        }

        private void Green_Click(object sender, RoutedEventArgs e)
        {
            if (_ControlBox != null)
            {
                TabItem tab = _ControlBox.SelectedItem as TabItem;
                RichTextBox RtextBox = tab.Content as RichTextBox;
                RtextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, Brushes.Green);
            }
        }

        private void _Open_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Text file| *.txt";
            //openFileDialog.Title = "Open text file";

            RichTextBox LehaGdeBruski = new RichTextBox();
            LehaGdeBruski.TextChanged += new TextChangedEventHandler(RichTextBox_TextChanged);


            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(LehaGdeBruski.Document.ContentStart, LehaGdeBruski.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }

        }


        //private void ClrPcker_Background_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        //{
        //    if (_ControlBox != null)
        //    {
        //        TabItem tab = _ControlBox.SelectedItem as TabItem;
        //        RichTextBox RtextBox = tab.Content as RichTextBox;
        //        Color? C = ClrPcker_Background.SelectedColor.Value as Color?;
        //        RtextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, Brush.C);
        //    }
        //}
        private void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file| *.txt";
            openFileDialog.Title = "Open text file";

            RichTextBox richTextBox = RichTExtBoxCreatetor.CreateRichTextBox(item_Click_Close,
                RichTextBox_DragOver,
                RichTextBox_Drop,
                RichTextBox_TextChanged
            );

            if (openFileDialog.ShowDialog() == true)
            {
                TabItemManipulate.NewTabItem(openFileDialog.FileName, richTextBox, _ControlBox);

                RichTextBox currentRichTextBox = TabItemManipulate.GetCurrentRichTextBox(_ControlBox);

                currentRichTextBox.Selection.Text = File.ReadAllText(openFileDialog.FileName);

                LastFileSaver.AddFilePath(openFileDialog.FileName);
            }
        }

        private void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            RichTextBox LehaGdeBruski = new RichTextBox();
            LehaGdeBruski.TextChanged += new TextChangedEventHandler(RichTextBox_TextChanged);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text file|.txt";
            saveFileDialog.Title = "Save text file";

            if (saveFileDialog.ShowDialog() == true)
            {
                RichTextBox currentRichText = TabItemManipulate.GetCurrentRichTextBox(_ControlBox);

                string myText = new TextRange(currentRichText.Document.ContentStart, currentRichText.Document.ContentEnd).Text;
                System.IO.File.WriteAllText(saveFileDialog.FileName, myText);
                MessageBox.Show("Файл сохранен");
            }
        }

        #region style готово
        private void changeStyle_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = e.Source as MenuItem;

            ResourceDictionary dict = new ResourceDictionary();
            switch (mi.Name)
            {
                case "changeStyle_dark":
                    dict.Source = new Uri("Style.Default.xaml", UriKind.Relative);
                    break;

                default:
                    dict.Source = new Uri("Resources/Style.White.xaml", UriKind.Relative);
                    break;
            }

            //3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
            ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                where d.Source != null && d.Source.OriginalString.StartsWith("Resources/Style.")
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
        }
        #endregion готово

        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;


            foreach (MenuItem i in menuLanguage.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }

        }

        private void ChangeTheme(string ThemeName)
        {
            Uri uri = new Uri(ThemeName + ".xaml", UriKind.Relative);
            ResourceDictionary resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }
        private void BlackTheme_Click(object sender, RoutedEventArgs e)
        {
            ChangeTheme("Black");
        }
        private void WhiteTheme_Click(object sender, RoutedEventArgs e)
        {
            ChangeTheme("White");
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Roflich lox");
        }
    }
}
