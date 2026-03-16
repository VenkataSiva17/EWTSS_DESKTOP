using System.Windows;
using System.Windows.Controls;

namespace EWTSS_DESKTOP.Presentation.Views.LogManagement
{
    public partial class LogManagementView : Page
    {
        public LogManagementView()
        {
            InitializeComponent();
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