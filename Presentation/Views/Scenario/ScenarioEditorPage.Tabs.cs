using System.Windows;
using System.Windows.Media;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioEditorPage
    {
        private void ShowCommEmitterTechnicalTab()
        {
            CommEmitterTechnicalScrollViewer.Visibility = Visibility.Visible;
            CommEmitterTacticalScrollViewer.Visibility = Visibility.Collapsed;

            CommEmitterTechnicalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#2CB7B0")!;
            CommEmitterTacticalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#179C96")!;
        }

        private void ShowCommEmitterTacticalTab()
        {
            CommEmitterTechnicalScrollViewer.Visibility = Visibility.Collapsed;
            CommEmitterTacticalScrollViewer.Visibility = Visibility.Visible;

            CommEmitterTechnicalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#179C96")!;
            CommEmitterTacticalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#2CB7B0")!;
        }

        private void CommEmitterTechnicalTabButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCommEmitterTechnicalTab();
        }

        private void CommEmitterTacticalTabButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCommEmitterTacticalTab();
        }

        private void ShowRadarEmitterTechnicalTab()
        {
            RadarEmitterTechnicalScrollViewer.Visibility = Visibility.Visible;
            RadarEmitterTacticalScrollViewer.Visibility = Visibility.Collapsed;

            RadarEmitterTechnicalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#2CB7B0")!;
            RadarEmitterTacticalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#179C96")!;
        }

        private void ShowRadarEmitterTacticalTab()
        {
            RadarEmitterTechnicalScrollViewer.Visibility = Visibility.Collapsed;
            RadarEmitterTacticalScrollViewer.Visibility = Visibility.Visible;

            RadarEmitterTechnicalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#179C96")!;
            RadarEmitterTacticalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#2CB7B0")!;
        }

        private void RadarEmitterTechnicalTabButton_Click(object sender, RoutedEventArgs e)
        {
            ShowRadarEmitterTechnicalTab();
        }

        private void RadarEmitterTacticalTabButton_Click(object sender, RoutedEventArgs e)
        {
            ShowRadarEmitterTacticalTab();
        }

        private void CommEmitterTechnicalTabButton2_Click(object sender, RoutedEventArgs e)
        {
            CommEmitterTechnicalScrollViewer2.Visibility = Visibility.Visible;
            CommEmitterTacticalScrollViewer2.Visibility = Visibility.Collapsed;

            CommEmitterTechnicalTabButton2.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#2CB7B0")!;
            CommEmitterTacticalTabButton2.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#179C96")!;
        }

        private void CommEmitterTacticalTabButton2_Click(object sender, RoutedEventArgs e)
        {
            CommEmitterTechnicalScrollViewer2.Visibility = Visibility.Collapsed;
            CommEmitterTacticalScrollViewer2.Visibility = Visibility.Visible;

            CommEmitterTechnicalTabButton2.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#179C96")!;
            CommEmitterTacticalTabButton2.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#2CB7B0")!;
        }

        private void RadarTechnicalTabButton2_Click(object sender, RoutedEventArgs e)
        {
            RadarTechnicalScrollViewer2.Visibility = Visibility.Visible;
            RadarTacticalScrollViewer2.Visibility = Visibility.Collapsed;

            RadarTechnicalTabButton2.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#2CB7B0")!;
            RadarTacticalTabButton2.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#179C96")!;
        }

        private void RadarTacticalTabButton2_Click(object sender, RoutedEventArgs e)
        {
            RadarTechnicalScrollViewer2.Visibility = Visibility.Collapsed;
            RadarTacticalScrollViewer2.Visibility = Visibility.Visible;

            RadarTechnicalTabButton2.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#179C96")!;
            RadarTacticalTabButton2.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#2CB7B0")!;
        }
    }
}