using System;
using System.Windows.Controls;
using System.Windows.Threading;
using EWTSS_DESKTOP.Presentation.ViewModels;
using EWTSS_DESKTOP.Helpers;

namespace EWTSS_DESKTOP.Presentation.Views.Dashboard
{
    public partial class ScenarioHomeView : Page
    {
        private readonly DispatcherTimer _clockTimer;
        public ScenarioHomeView(ScenarioDashboardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            _clockTimer = ClockHelper.StartClock(TimeText);
        }

    }
}