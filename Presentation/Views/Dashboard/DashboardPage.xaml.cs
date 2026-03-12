using System.Windows.Controls;
using EWTSS_DESKTOP.Models;

namespace EWTSS_DESKTOP.Presentation.Views.Dashboard
{
    public partial class DashboardPage : Page
    {
        private User _loggedInUser;

        public DashboardPage(User user)
        {
            InitializeComponent();
            _loggedInUser = user;
            WelcomeLabel.Content = $"Welcome, {_loggedInUser.FirstName} {_loggedInUser.LastName}!";
        }
    }
}