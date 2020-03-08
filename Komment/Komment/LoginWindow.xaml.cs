using System.Windows;

namespace Komment
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if(Username.Text.Length < 2|| Password.Password.Length < 2)
            {
                WarningText.Text = "Password or username too short";
                WarningText.Visibility = Visibility.Visible;
                return;
            }
            WarningText.Text = "Logging you in...";
            WarningText.Visibility = Visibility.Visible;

            User.username = Username.Text;
            User.password = Password.Password;
            string response = await User.LogInAsync();
            if(response == null)
            {
                Close();
            }
            else
            {
                WarningText.Text = response;
                WarningText.Visibility = Visibility.Visible;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
        }

        private void Input_Changed(object sender, RoutedEventArgs e)
        {
            WarningText.Visibility = Visibility.Hidden;
        }
    }
}
