using System.Windows.Controls;
using EWTSS_DESKTOP.Helpers;
using System.Windows.Threading; 

namespace EWTSS_DESKTOP.Presentation.Views.Report
{
    public partial class ReportView : Page
    {
        private readonly DispatcherTimer _clockTimer;
        public ReportView()
        {
            InitializeComponent();
            _clockTimer = ClockHelper.StartClock(TimeText);
        }
    }
}