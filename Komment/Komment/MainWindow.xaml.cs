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
            InitializeUser();
        }

        private async void InitializeUser()
        {
            var userLoggedIn = await UserData.LoadUser();
            if (userLoggedIn)
                NetworkHandler.Initialize();
            else
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
            }
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
