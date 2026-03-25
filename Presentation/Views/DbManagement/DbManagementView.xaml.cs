using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EWTSS_DESKTOP.Helpers;
using System.Windows.Threading;

using WpfButton = System.Windows.Controls.Button;
using WpfBrushes = System.Windows.Media.Brushes;
using WpfRectangle = System.Windows.Shapes.Rectangle;

namespace EWTSS_DESKTOP.Presentation.Views.DbManagement
{
    public partial class DbManagementView : Page
    {
        private readonly DispatcherTimer _clockTimer;

        public DbManagementView()
        {
            InitializeComponent();
            _clockTimer = ClockHelper.StartClock(TimeText);
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is WpfButton button)
            {
                var border = button.Template.FindName("UploadBorder", button) as WpfRectangle;
                if (border != null)
                {
                    border.Stroke = WpfBrushes.DeepSkyBlue;
                }
            }
        }
    }
}