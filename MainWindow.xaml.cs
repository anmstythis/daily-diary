using System.Text;
using System.Windows;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace daily_diary
{
    internal class JsonSave
    {
        public static void jsonSerialize<T>(T textfile, string pathjson)
        {
            if (!File.Exists(pathjson))
            {
                File.Create(pathjson).Close();
            }
            string txtjson = JsonConvert.SerializeObject(textfile, Formatting.Indented);
            File.WriteAllText(pathjson, txtjson);
        }
        public static T jsonDeserialize<T>(string pathjson)
        {
            if(File.Exists(pathjson))
            {
                var txtjson = File.ReadAllText(pathjson);
                T result = JsonConvert.DeserializeObject<T>(txtjson);
                return result;
            }
            else
            {
                return default(T);
            }
        }

        public static void removeObject(string pathjson, int index)
        {
            if(File.Exists(pathjson))
            {
                var forDelete = JsonSave.jsonDeserialize<List<Note>>(pathjson);
                forDelete.RemoveAt(index);
                jsonSerialize(forDelete, pathjson);
            }
        }
    }

    public partial class MainWindow : Window
    {
        public static string pathjson = "C:\\Users\\My\\OneDrive\\Документы\\конспекты\\ОАиП C#\\daily_diary\\daily_diary\\notes.json";
        List<Note> notes = new List<Note>();
        public MainWindow()
        {
            InitializeComponent();
            calendar.SelectedDate = DateTime.Today;
            outputbox.DisplayMemberPath = "Title";
        }
        private void showNotes()
        {
            if(File.Exists(pathjson))
            {
                var result = JsonSave.jsonDeserialize<List<Note>>(pathjson);
                DateTime dateSelected = calendar.SelectedDate ?? DateTime.Today;
                List<Note> notesDisplayed = result.FindAll(note => note.Date == dateSelected.Date);
                outputbox.ItemsSource = notesDisplayed;
            }
        }
        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            Note jsonfile = new Note((DateTime)calendar.SelectedDate, titlebox.Text, description.Text);
            notes.Add(jsonfile);

            if (titlebox.Text != string.Empty || description.Text != string.Empty)
            {
                if (!File.Exists(pathjson))
                {
                    JsonSave.jsonSerialize(notes, pathjson);
                }
                else
                {
                    Note jsonfile_new = new Note((DateTime)calendar.SelectedDate, titlebox.Text, description.Text);
                    var forAppend = JsonSave.jsonDeserialize<List<Note>>(pathjson);
                    forAppend.Add(jsonfile_new);
                    JsonSave.jsonSerialize(forAppend, pathjson);
                }
            }
            showNotes();
            titlebox.Text = string.Empty;
            description.Text = string.Empty;
        }
        private void deletebutton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(pathjson))
            {
                int index = outputbox.SelectedIndex;
                JsonSave.removeObject(pathjson, index);
            }
            showNotes();
            titlebox.Text = string.Empty;
            description.Text = string.Empty;
        }

        private void updatebutton_Click(object sender, RoutedEventArgs e)
        {
            Note jsonfile = new Note((DateTime)calendar.SelectedDate, titlebox.Text, description.Text);

            if (File.Exists(pathjson) && calendar.SelectedDate == jsonfile.Date)
            {
                var forUpdate = JsonSave.jsonDeserialize<List<Note>>(pathjson);

                int index = outputbox.SelectedIndex;
                JsonSave.removeObject(pathjson, index);
                forUpdate[index] = jsonfile;
                JsonSave.jsonSerialize(forUpdate, pathjson);
            }
            showNotes();
        }

        private void outputbox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Note selectedNote = (Note)outputbox.SelectedItem;
            if (selectedNote != null)
            {
                titlebox.Text = selectedNote.Title;
                description.Text = selectedNote.Description;
            }
        }
        private void calendar_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            titlebox.Text = string.Empty;
            description.Text = string.Empty;
            showNotes();
        }
    }
}