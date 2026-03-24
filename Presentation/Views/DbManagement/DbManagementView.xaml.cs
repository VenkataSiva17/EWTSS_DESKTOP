using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using EWTSS_DESKTOP.Helpers;
using System.Windows.Threading;



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
            if (sender is Button button)
            {
                var border = button.Template.FindName("UploadBorder", button) as Rectangle;
                if (border != null)
                {
                    border.Stroke = Brushes.DeepSkyBlue;
                }
            }
        }
    }
}