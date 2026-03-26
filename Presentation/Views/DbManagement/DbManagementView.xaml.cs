using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
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

            var repository = new DbManagementRepository(
                "localhost",
                "ewtss_db_march",
                "root",
                "DDD12");

            var service = new DbManagementService(repository);

            DataContext = new DbManagementViewModel(service);
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