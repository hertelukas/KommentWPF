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
    /// Interaction logic for FolderPage.xaml
    /// </summary>
    public partial class FolderPage : Page
    {
        public List<Folder> folders = new List<Folder>();

        List<string> currentFolderStringList = new List<string>();

        private int _highestLevel = 0;
        public FolderPage()
        {
            InitializeComponent();
            NetworkHandler.NotesLoaded += SetFolders;
        }
        
        private void SetFolders(object sender, EventArgs e)
        {
            FolderView1.Visibility = Visibility.Visible;
            FolderView2.Visibility = Visibility.Collapsed;
            
            foreach (var note in UserData.Notes)
            {
                int i = 0;
                foreach (string folderName in note.Folders)
                {
                    Folder folder = new Folder
                    {
                        Name = folderName,
                        Level = i
                    };
                    if (folder.Level != 0)
                        folder.ParentFolder = folders[folders.Count - 1];

                    folders.Add(folder);
                    if (_highestLevel < i)
                        _highestLevel = i;
                    i++;
                }
            }

            foreach (var folder in folders)
            {
                if (folder.Level == 0)
                    currentFolderStringList.Add(folder.Name);
            }


            FolderView1.ItemsSource = currentFolderStringList;
            
        }

        private void FolderView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FolderView2.ItemsSource = null;
            FolderView1.Width = 100;
            FolderView2.Visibility = Visibility.Visible;
            currentFolderStringList.Clear();

            //Find the note 
            foreach (var folder in folders)
            {
                if (folder.Level == 1 && folder.ParentFolder.Name == FolderView1.SelectedValue.ToString())
                    currentFolderStringList.Add(folder.Name);
            }


            FolderView2.ItemsSource = currentFolderStringList;
        }
    }
}
