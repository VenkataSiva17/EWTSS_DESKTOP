using System.Windows.Controls;
using EWTSS_DESKTOP.Presentation.ViewModels;

namespace EWTSS_DESKTOP.Presentation.Views.Dashboard
{
    public partial class ScenarioHomeView : Page
    {
        public ScenarioHomeView(ScenarioDashboardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}