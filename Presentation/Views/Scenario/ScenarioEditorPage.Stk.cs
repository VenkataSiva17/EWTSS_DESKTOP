using EWTSS_DESKTOP.Infrastructure.Services;
using System;
using System.Windows;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;
using WpfBrush = System.Windows.Media.Brush;
using WpfBrushConverter = System.Windows.Media.BrushConverter;
using WpfBrushes = System.Windows.Media.Brushes;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioEditorPage
    {
        private void InitializeStkViews(string scenarioName)
        {
            try
            {
                _stkEngineService.Initialize();

                _stk3DView = new Stk3DViewControl();
                _stk2DView = new Stk2DViewControl();

                Host3D.Child = _stk3DView;
                Host2D.Child = _stk2DView;

                _stk3DView.CreateScenario(scenarioName);
                _stk2DView.CreateScenario(scenarioName);

                Show3D();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"STK visualization could not be initialized.\n\n{ex.Message}",
                    "STK Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Host3D.Visibility = Visibility.Collapsed;
                Host2D.Visibility = Visibility.Collapsed;

                Placeholder3DText.Visibility = Visibility.Visible;
                Placeholder2DText.Visibility = Visibility.Collapsed;
            }
        }

        private void Show3D()
        {
            Host3D.Visibility = Visibility.Visible;
            Host2D.Visibility = Visibility.Collapsed;

            Placeholder3DText.Visibility = Visibility.Collapsed;
            Placeholder2DText.Visibility = Visibility.Collapsed;

            Btn3D.Background = (WpfBrush)new WpfBrushConverter().ConvertFromString("#062235")!;
            Btn2D.Background = WpfBrushes.Transparent;
        }

        private void Show2D()
        {
            Host3D.Visibility = Visibility.Collapsed;
            Host2D.Visibility = Visibility.Visible;

            Placeholder3DText.Visibility = Visibility.Collapsed;
            Placeholder2DText.Visibility = Visibility.Collapsed;

            Btn2D.Background = (WpfBrush)new WpfBrushConverter().ConvertFromString("#062235")!;
            Btn3D.Background = WpfBrushes.Transparent;
        }

        private void Btn3D_Click(object sender, RoutedEventArgs e)
        {
            Show3D();
        }

        private void Btn2D_Click(object sender, RoutedEventArgs e)
        {
            Show2D();
        }
    }
}