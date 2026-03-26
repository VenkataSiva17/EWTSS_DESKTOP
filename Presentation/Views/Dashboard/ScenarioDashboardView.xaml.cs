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
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new UserManagementView();
            SetActiveMenu(UserManagementButton);
        }

        private void DbManagement_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new DbManagementView();
            SetActiveMenu(DbManagementButton);
        }

        private void LogManagement_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new LogManagementView();
            SetActiveMenu(LogManagementButton);
        }

        private void EmitterLibrary_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new EmitterLibraryView();
            SetActiveMenu(EmitterLibraryButton);
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ReportView();
            SetActiveMenu(ReportButton);
        }

        private void IpConfiguration_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new IpConfigurationView();
            SetActiveMenu(IpConfigurationButton);
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ReplayView();
            SetActiveMenu(ReplayButton);
        }

        private void SetActiveMenu(WpfButton activeButton)
        {
            HomeButton.Background = WpfBrushes.Transparent;
            UserManagementButton.Background = WpfBrushes.Transparent;
            DbManagementButton.Background = WpfBrushes.Transparent;
            LogManagementButton.Background = WpfBrushes.Transparent;
            EmitterLibraryButton.Background = WpfBrushes.Transparent;
            ReportButton.Background = WpfBrushes.Transparent;
            IpConfigurationButton.Background = WpfBrushes.Transparent;
            ReplayButton.Background = WpfBrushes.Transparent;

            activeButton.Background = (WpfBrush)new WpfBrushConverter().ConvertFromString("#169C96");
        }
    }
}