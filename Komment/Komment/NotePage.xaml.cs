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
    /// Interaction logic for NotePage.xaml
    /// </summary>
    public partial class NotePage : Page
    {
        private readonly Note _currentNote;

        public NotePage(Note note)
        {
            InitializeComponent();

            _currentNote = note;
            NoteTitle.Text = note.Title;
            Content.Text = note.Content;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(Content.Text) && !String.IsNullOrEmpty(NoteTitle.Text))
            {
                _currentNote.Content = Content.Text;
                _currentNote.Title = NoteTitle.Text;

                UpdateNoteResponse response = await NetworkHandler.UpdateNoteAsync(_currentNote);

                if (response == UpdateNoteResponse.Success)
                {
                    MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                    mainWindow.FullScreenFrame.Content = null;

                    mainWindow.homePage.UpdateNotesList();
                }
            }

        }
    }
}
