using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EWTSS_DESKTOP.Presentation.Views.DbManagement
{
    public partial class DbManagementView : Page
    {
        public DbManagementView()
        {
            InitializeComponent();
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