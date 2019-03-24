using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Text_redactor_WPF
{
    [DataContract]
    public class LastFiles
    {
        [DataMember]
        private StringBuilder filesPath = new StringBuilder(" ", 256);

        [DataMember]
        private DateTime date = new DateTime();

        public LastFiles()
        {
            date = DateTime.Now;
        }

        //[DataMember]
        public string Path
        {
            set
            {
                filesPath.Replace(filesPath.ToString(), value);
            }
            get => filesPath.ToString();
        }

        //[DataMember]
        public DateTime Date
        {
            get => date;
            set => date = value;
        }
    }
    class LastFileSaver
    {
        [DataMember]
        static List<LastFiles> filesPath = new List<LastFiles>();

        public static void GetFiles()
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<LastFiles>));
            using (FileStream stream = new FileStream("lastFiles.json", FileMode.Open))
            {
                filesPath = (List<LastFiles>)jsonFormatter.ReadObject(stream);//
            }
        }

        public static void SetFiles()
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<LastFiles>));

            using (FileStream stream = new FileStream("lastFiles.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(stream, filesPath);
            }
        }

        public static void AddFilePath(string path)
        {
            bool pathAdded = false;
            int oldestDateIndex = 0;

            if (filesPath.Count < 5)
                filesPath.Add(new LastFiles());

            for (int i = 0; i < filesPath.Count && i < 5; i++)
            {
                if (filesPath[i].Path == " ")
                {
                    filesPath[i].Path = path;
                    pathAdded = true;
                    break;
                }
            }

            if (!pathAdded)
            {
                for (int i = 1; i < filesPath.Count && i < 5; i++)
                {
                    if (filesPath[i - 1].Date > filesPath[i].Date)
                    {
                        oldestDateIndex = i;
                    }
                }
                filesPath[oldestDateIndex].Path = path;
            }
        }

        public static void OpenFiles(TabControl tabControl,
            RoutedEventHandler contextMenuEventHandler_Click,
            DragEventHandler richTextBoxEventHandler_DragOver,
            DragEventHandler richTextBoxEventHandler_DragDrop,
            TextChangedEventHandler richTextBoxEventHandler_OnTextChange)
        {
            for (int i = 0; i < filesPath.Count && i < 5; i++)
            {
                RichTextBox richTextBox = RichTExtBoxCreatetor.CreateRichTextBox(contextMenuEventHandler_Click,
                                                               richTextBoxEventHandler_DragOver,
                                                               richTextBoxEventHandler_DragDrop,
                                                               richTextBoxEventHandler_OnTextChange
                                                               );


                TabItemManipulate.NewTabItem(filesPath[i].Path, richTextBox, tabControl);
                tabControl.SelectedIndex = i;
                RichTextBox currentRichTextBox = TabItemManipulate.GetCurrentRichTextBox(tabControl);

                currentRichTextBox.Selection.Text = File.ReadAllText(filesPath[i].Path);
            }
        }
    }
}
