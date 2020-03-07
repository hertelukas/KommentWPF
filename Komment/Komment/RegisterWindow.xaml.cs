using System.Windows;

namespace Komment
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            WarningText.Visibility = Visibility.Hidden;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if(Password.Password == PasswordConfirm.Password && Username.Text.Length > 4 && Password.Password.Length > 4)
            {
                User.username = Username.Text;
                User.password = Password.Password;
                string response = await User.RegisterAsync();
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
            else if(Password.Password != PasswordConfirm.Password)
            {
                WarningText.Text = "Passwords do not match!";
                WarningText.Visibility = Visibility.Visible;
            }
            else if(Username.Text.Length <= 4)
            {
                WarningText.Text = "Username is too short!";
                WarningText.Visibility = Visibility.Visible;
            }
            else
            {
                WarningText.Text = "Password is too short!";
                WarningText.Visibility = Visibility.Visible;
            }
        }

        private void Input_Changed(object sender, RoutedEventArgs e)
        {
            WarningText.Visibility = Visibility.Hidden;
        }
    }
}
