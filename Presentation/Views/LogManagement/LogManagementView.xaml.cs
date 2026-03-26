using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EWTSS_DESKTOP.Presentation.ViewModels;
using EWTSS_DESKTOP.Helpers;
using System.Windows.Threading;

namespace EWTSS_DESKTOP.Presentation.Views.LogManagement
{
    public partial class LogManagementView : Page
    { 
        private readonly DispatcherTimer _clockTimer;
        private readonly LogManagementViewModel _viewModel;

        public LogManagementView()
        {
            InitializeComponent();

            _viewModel = new LogManagementViewModel();
            DataContext = _viewModel;
           _clockTimer = ClockHelper.StartClock(TimeText);


            SetActiveTab(UserTab);
        }

        private void ResetTabs()
        {
            var normalBrush = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#179C96");

            UserTab.Background = normalBrush;
            UserTab.Foreground = System.Windows.Media.Brushes.Black;
            UserTab.FontWeight = FontWeights.Normal;

            SentToDrsTab.Background = normalBrush;
            SentToDrsTab.Foreground = System.Windows.Media.Brushes.Black;
            SentToDrsTab.FontWeight = FontWeights.Normal;

            ReceivedFromDrsTab.Background = normalBrush;
            ReceivedFromDrsTab.Foreground = System.Windows.Media.Brushes.Black;
            ReceivedFromDrsTab.FontWeight = FontWeights.Normal;

            SystemTab.Background = normalBrush;
            SystemTab.Foreground = System.Windows.Media.Brushes.Black;
            SystemTab.FontWeight = FontWeights.Normal;


        }

        private void SetActiveTab(System.Windows.Controls.Button activeButton)
        {
            ResetTabs();

            activeButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#27B7AE");
            activeButton.Foreground = System.Windows.Media.Brushes.White;
            activeButton.FontWeight = FontWeights.Bold;
        }

        private void UserTab_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SetUserTab();
            SetActiveTab(UserTab);
        }

        private void SentToDrsTab_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SetSentToDrsTab();
            SetActiveTab(SentToDrsTab);
        }

        private void ReceivedFromDrsTab_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SetReceivedFromDrsTab();
            SetActiveTab(ReceivedFromDrsTab);
        }

        private void SystemTab_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SetSystemTab();
            SetActiveTab(SystemTab);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.SearchText = SearchBox.Text;

            SearchPlaceholder.Visibility =
                _viewModel.IsSearchPlaceholderVisible
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        private void UserManagementView_Unloaded(object sender, RoutedEventArgs e)
        {
            _clockTimer.Stop();
        }


    }
}