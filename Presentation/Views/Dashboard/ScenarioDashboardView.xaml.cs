using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Helpers;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Presentation.ViewModels;
using EWTSS_DESKTOP.Presentation.Views.DbManagement;
using EWTSS_DESKTOP.Presentation.Views.EmitterLibrary;
using EWTSS_DESKTOP.Presentation.Views.IpConfiguration;
using EWTSS_DESKTOP.Presentation.Views.LogManagement;
using EWTSS_DESKTOP.Presentation.Views.Login;
using EWTSS_DESKTOP.Presentation.Views.Replay;
using EWTSS_DESKTOP.Presentation.Views.Report;
using EWTSS_DESKTOP.Presentation.Views.UserManagement;

using WpfButton = System.Windows.Controls.Button;
using WpfBrush = System.Windows.Media.Brush;
using WpfBrushes = System.Windows.Media.Brushes;
using WpfBrushConverter = System.Windows.Media.BrushConverter;

namespace EWTSS_DESKTOP.Presentation.Views.Dashboard
{
    public partial class ScenarioDashboardView : Page
    {
        private readonly ScenarioDashboardViewModel _viewModel;
        private readonly User _loggedInUser;
        private readonly StkEngineService _stkEngineService;

        public ScenarioDashboardView(User user, StkEngineService stkEngineService)
        {
            InitializeComponent();

            _loggedInUser = user;
            _stkEngineService = stkEngineService;

            _viewModel = new ScenarioDashboardViewModel(user, stkEngineService);
            DataContext = _viewModel;

            MainContentFrame.Content = new ScenarioHomeView(_viewModel);
            SetActiveMenu(HomeButton);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ScenarioHomeView(_viewModel);
            SetActiveMenu(HomeButton);
            CloseUserMenu();
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new UserManagementView();
            SetActiveMenu(UserManagementButton);
            CloseUserMenu();
        }

        private void DbManagement_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new DbManagementView();
            SetActiveMenu(DbManagementButton);
            CloseUserMenu();
        }

        private void LogManagement_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new LogManagementView();
            SetActiveMenu(LogManagementButton);
            CloseUserMenu();
        }

        private void EmitterLibrary_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new EmitterLibraryView();
            SetActiveMenu(EmitterLibraryButton);
            CloseUserMenu();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ReportView();
            SetActiveMenu(ReportButton);
            CloseUserMenu();
        }

        private void IpConfiguration_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new IpConfigurationView();
            SetActiveMenu(IpConfigurationButton);
            CloseUserMenu();
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ReplayView();
            SetActiveMenu(ReplayButton);
            CloseUserMenu();
        }

        private void UserMenuButton_Click(object sender, MouseButtonEventArgs e)
        {
            UserDropdownPopup.IsOpen = !UserDropdownPopup.IsOpen;
            e.Handled = true;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            CloseUserMenu();
            System.Windows.MessageBox.Show("Change Password clicked.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SGConfig_Click(object sender, RoutedEventArgs e)
        {
            CloseUserMenu();
            System.Windows.MessageBox.Show("SG Config clicked.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            CloseUserMenu();

            var result = System.Windows.MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            UserSession.CurrentUser = null!;

            if (System.Windows.Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                mainWindow.MainFrame.Navigate(new LoginView());
            }
            else
            {
                NavigationService?.Navigate(new LoginView());
            }
        }

        private void CloseUserMenu()
        {
            if (UserDropdownPopup != null)
            {
                UserDropdownPopup.IsOpen = false;
            }
        }

        private void SetActiveMenu(WpfButton activeButton)
        {
            WpfButton[] menuButtons =
            {
                HomeButton,
                UserManagementButton,
                DbManagementButton,
                LogManagementButton,
                EmitterLibraryButton,
                ReportButton,
                IpConfigurationButton,
                ReplayButton
            };

            foreach (var button in menuButtons)
            {
                button.Background = WpfBrushes.Transparent;
            }

            activeButton.Background = (WpfBrush)new WpfBrushConverter().ConvertFromString("#169C96");
        }
    }
}