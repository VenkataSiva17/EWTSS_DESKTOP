using System.Windows.Controls;
using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Presentation.ViewModels;

namespace EWTSS_DESKTOP.Presentation.Views.Dashboard
{
    public partial class DashboardPage : Page
    {
        public DashboardPage(User user)
        {
            InitializeComponent();
            DataContext = new DashboardViewModel(user);
        }
    }
}