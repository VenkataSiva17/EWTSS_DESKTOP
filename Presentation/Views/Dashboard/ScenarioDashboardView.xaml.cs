using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Presentation.ViewModels;
using EWTSS_DESKTOP.Presentation.Views.DbManagement;
using EWTSS_DESKTOP.Presentation.Views.Dashboard;
using EWTSS_DESKTOP.Presentation.Views.UserManagement;
using EWTSS_DESKTOP.Presentation.Views.LogManagement;

namespace EWTSS_DESKTOP.Presentation.Views.Dashboard
{
    public partial class ScenarioDashboardView : Page
    {
        private readonly ScenarioDashboardViewModel _viewModel;

        public ScenarioDashboardView(User user)
        {
            InitializeComponent();

            _viewModel = new ScenarioDashboardViewModel(user);
            DataContext = _viewModel;

            MainContentFrame.Navigate(new ScenarioHomeView(_viewModel));
            SetActiveMenu(HomeButton);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new ScenarioHomeView(_viewModel));
            SetActiveMenu(HomeButton);
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new UserManagementView());
            SetActiveMenu(UserManagementButton);
        }

        private void DbManagement_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new DbManagementView());
            SetActiveMenu(DbManagementButton);
        }

        private void LogManagement_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new LogManagementView());
            SetActiveMenu(LogManagementButton);
        }

        private void SetActiveMenu(Button activeButton)
        {
            HomeButton.Background = Brushes.Transparent;
            UserManagementButton.Background = Brushes.Transparent;
            DbManagementButton.Background = Brushes.Transparent;
            LogManagementButton.Background = Brushes.Transparent;

            activeButton.Background = (Brush)new BrushConverter().ConvertFromString("#169C96");
        }
    }
}