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
        readonly UserPage userPage = new UserPage();
        readonly SettingsPage settingsPage = new SettingsPage();
        public readonly HomePage homePage = new HomePage();
        readonly FolderPage folderPage = new FolderPage();

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
                UpdateUIAfterLogin();
                NetworkHandler.Initialize();
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
        public void UpdateUIAfterLogin()
        {
            UsernameTextBlock.Text = $"Hello {User.username}!";
        }


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
                    MainWindowFrame.Content = folderPage;
                    break;

                case 3:
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
