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
        private List<string> folders = new List<string>();
        public FolderPage()
        {
            InitializeComponent();
            NetworkHandler.NotesLoaded += SetFolders;
        }
        
        private void SetFolders(object sender, EventArgs e)
        {
            foreach (var note in UserData.Notes)
            {
                foreach (string folder in note.Folders)
                {
                    folders.Add(folder);
                    _ = Logger.LogInfo(folder);
                }
            }

            FolderView.ItemsSource = folders;
        }
    }
}
