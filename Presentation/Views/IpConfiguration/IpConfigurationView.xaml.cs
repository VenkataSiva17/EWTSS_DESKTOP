using System.Windows.Controls;
using EWTSS_DESKTOP.Helpers;
using System.Windows.Threading;
namespace EWTSS_DESKTOP.Presentation.Views.IpConfiguration
{
    public partial class IpConfigurationView : Page
    { 
        private readonly DispatcherTimer _clockTimer;
        public IpConfigurationView()
        {
            InitializeComponent();
            _clockTimer = ClockHelper.StartClock(TimeText);
        }
    }
}