using System.Windows.Controls;
using EWTSS_DESKTOP.Helpers;
using System.Windows.Threading; 

namespace EWTSS_DESKTOP.Presentation.Views.Replay
{
    public partial class ReplayView : Page
    {
        private readonly DispatcherTimer _clockTimer;
        public ReplayView()
        {
            InitializeComponent();
            _clockTimer = ClockHelper.StartClock(TimeText);

        }
    }
}