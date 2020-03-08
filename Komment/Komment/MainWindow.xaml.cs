using System;
using System.Collections.Generic;
using System.Windows;

namespace Komment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserPage userPage = new UserPage();
        SettingsPage settingsPage = new SettingsPage();
        HomePage homePage = new HomePage();

        public MainWindow()
        {
            InitializeComponent();
            InitializeUser();
            Logger.InitializeFolders();
            InitializeUI();
        }

        private async void InitializeUser()
        {
            var userLoggedInLocal = await UserData.LoadUser();
            if (userLoggedInLocal)
            {
                UsernameTextBlock.Text = $"Hello {User.username}!";
                NetworkHandler.Initialize();
                await NetworkHandler.LoadAllNotesAsync();
            }
            else
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
            }

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
            LoadPage(index);
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, 60 + (60 * index), 0, 0);
        }

        private void LoadPage(int index)
        {
            switch (index)
            {
                case 0:
                    MainWindowFrame.Content = userPage;
                    break;

                case 1:
                    MainWindowFrame.Content = homePage;
                    break;

                case 2:
                    MainWindowFrame.Content = settingsPage;
                    break;

                default:
                    break;
            }
        }

        private void InitializeUI()
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        #endregion

    }
}
