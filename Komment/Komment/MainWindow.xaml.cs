using System;
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
            NetworkHandler.LoggedIn += DownloadUserData;

            var userLoggedIn = await UserData.LoadUser();
            if (userLoggedIn)
            {
                NetworkHandler.Initialize();
                UsernameTextBlock.Text = $"Hello {User.username}!";
            }
            else
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
            }

        }

        private async void DownloadUserData(object source, EventArgs e)
        {
            UserData.notes = await NetworkHandler.LoadAllNotesAsync();
        }

        #region UI Elements

        //Buttons
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Other UI
        private void TopBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ListViewMenu_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, 60 + (60 * index), 0, 0);
        }

        #endregion

    }
}
