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

            NetworkHandler.Initialize();
            InitializeNetworkConnection();

            if (!User.IsLoggedIn)
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
