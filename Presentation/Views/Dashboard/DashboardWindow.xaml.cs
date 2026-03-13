using System.Windows;
using EWTSS_DESKTOP.Core.Models;

namespace EWTSS_DESKTOP.Presentation.Views.Dashboard
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow(User user)
        {
            InitializeComponent();

            // Load DashboardPage inside the frame
            MainFrame.Navigate(new DashboardPage(user));
        }
    }
}   