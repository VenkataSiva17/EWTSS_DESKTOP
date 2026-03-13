using System.Windows;
using EWTSS_DESKTOP.Infrastructure.Repositories;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Infrastructure.Data;
using EWTSS_DESKTOP.Presentation.ViewModels;
using EWTSS_DESKTOP.Presentation.Views.Login;
using EWTSS_DESKTOP.Presentation.Views.Dashboard;
using EWTSS_DESKTOP.Core.Models;

namespace EWTSS_DESKTOP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var db = new AppDbContext();
            var userService = new UserService(new UserRepository(db));

            var loginVM = new LoginViewModel(userService);
            var loginPage = new LoginView { DataContext = loginVM };

            loginVM.LoginSucceeded += (user) =>
            {
                var dashboard = new DashboardPage(user);
                MainFrame.Navigate(dashboard);
            };

            MainFrame.Navigate(loginPage);
        }
    }
}