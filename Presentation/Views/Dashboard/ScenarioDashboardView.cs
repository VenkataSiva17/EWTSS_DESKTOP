using System.Windows.Controls;
using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Presentation.ViewModels;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioDashboardView : Page
    {
        public ScenarioDashboardView(User user)
        {
            InitializeComponent();
            DataContext = new ScenarioDashboardViewModel(user);
        }
    }
}