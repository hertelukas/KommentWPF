using System.Windows;

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

            if (!User.IsLoggedIn)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
            }

            NetworkHandler.Initialize();
            InitializeNetworkConnection();
        }

        private async void InitializeNetworkConnection()
        {
            var notes = await NetworkHandler.LoadAllNotesAsync();

            if (notes != null)
            {
                User.Data.notes = notes;
                _ = Logger.LogInfo($"Loaded {User.Data.notes.Count.ToString()} notes");
            }
        }
    }
}
