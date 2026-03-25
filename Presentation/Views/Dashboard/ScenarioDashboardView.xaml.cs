using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Presentation.ViewModels;
using EWTSS_DESKTOP.Presentation.Views.DbManagement;
using EWTSS_DESKTOP.Presentation.Views.UserManagement;
using EWTSS_DESKTOP.Presentation.Views.LogManagement;
using EWTSS_DESKTOP.Presentation.Views.EmitterLibrary;
using EWTSS_DESKTOP.Presentation.Views.Report;
using EWTSS_DESKTOP.Presentation.Views.IpConfiguration;
using EWTSS_DESKTOP.Presentation.Views.Replay;

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

        private void EmitterLibrary_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new EmitterLibraryView());
            SetActiveMenu(EmitterLibraryButton);
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new ReportView());
            SetActiveMenu(ReportButton);
        }

        private void IpConfiguration_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new IpConfigurationView());
            SetActiveMenu(IpConfigurationButton);
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new ReplayView());
            SetActiveMenu(ReplayButton);
        }

        private void SetActiveMenu(Button activeButton)
        {
            HomeButton.Background = Brushes.Transparent;
            UserManagementButton.Background = Brushes.Transparent;
            DbManagementButton.Background = Brushes.Transparent;
            LogManagementButton.Background = Brushes.Transparent;
            EmitterLibraryButton.Background = Brushes.Transparent;
            ReportButton.Background = Brushes.Transparent;
            IpConfigurationButton.Background = Brushes.Transparent;
            ReplayButton.Background = Brushes.Transparent;

            activeButton.Background = (Brush)new BrushConverter().ConvertFromString("#169C96");
        }
    }
}