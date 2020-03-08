using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Komment
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            NetworkHandler.NotesLoaded += OnNotesLoaded;
        }

        public void OnNotesLoaded(object source, EventArgs e)
        {
            UpdateNotesList();
        }

        private void UpdateNotesList()
        {
            NoteView.ItemsSource = UserData.Notes;
        }

        private void NoteView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NoteView.SelectedIndex != -1)
            {
                var index = NoteView.SelectedIndex;
                Note note = UserData.Notes[index];
                NoteView.SelectedIndex = -1;
            }
        }
    }
}
