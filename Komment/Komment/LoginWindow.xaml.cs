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
            User.username = Username.Text;
            User.password = Password.Password;
            await User.LogInAsync();
            Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
        }
    }
}
