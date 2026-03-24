using System.Windows;
using System.Windows.Controls;
using EWTSS_DESKTOP.Helpers;
using System.Windows.Threading; 

namespace EWTSS_DESKTOP.Presentation.Views.LogManagement
{
    public partial class LogManagementView : Page
    {
        private readonly DispatcherTimer _clockTimer;
        public LogManagementView()
        {
            InitializeComponent();
            _clockTimer = ClockHelper.StartClock(TimeText);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility =
                string.IsNullOrWhiteSpace(SearchBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}