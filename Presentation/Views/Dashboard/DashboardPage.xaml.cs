using System.Windows.Controls;
using EWTSS_DESKTOP.Core.Models;

namespace EWTSS_DESKTOP.Presentation.Views.Dashboard
{
    public partial class DashboardPage : Page
    {
        private User _user;

        public DashboardPage(User user)
        {
            InitializeComponent();
            _user = user;
        }
    }
}