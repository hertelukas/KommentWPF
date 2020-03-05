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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Test user data
            //Here we have to first initialize the User
            User.username = "lukas3";
            User.password = "ferkel";

            NetworkHandler.Initialize();
            InitializeNetworkConnection();


        }

        private async void InitializeNetworkConnection()
        {
            var notes = await NetworkHandler.LoadAllNotesAsync();

            if(notes != null)
            {
                User.Data.notes = notes;
                await Logger.LogInfo($"Loaded {User.Data.notes.Count.ToString()} notes");
            }
        }
    }
}
