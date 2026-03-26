using System.Windows;
using System.Windows.Controls;
using EWTSS_DESKTOP.Helpers;
using System.Windows.Threading;

namespace EWTSS_DESKTOP.Presentation.Views.EmitterLibrary
{
    public partial class EmitterLibraryView : Page
    {
        private readonly DispatcherTimer _clockTimer;

        public EmitterLibraryView()
        {
            InitializeComponent();
            UpdatePlaceholder();
            _clockTimer = ClockHelper.StartClock(TimeText);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholder();
        }

        private void UpdatePlaceholder()
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
                SearchPlaceholder.Visibility = Visibility.Visible;
            else
                SearchPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void CreateEmitter_Click(object sender, RoutedEventArgs e)
        {
            CreateEmitterOverlay.Visibility = Visibility.Visible;
        }

        private void CloseCreateEmitter_Click(object sender, RoutedEventArgs e)
        {
            CreateEmitterOverlay.Visibility = Visibility.Collapsed;
        }
    }
}