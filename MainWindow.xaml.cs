using System.Windows;
using EWTSS_DESKTOP.Infrastructure.Repositories;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Infrastructure.Data;
using EWTSS_DESKTOP.Presentation.ViewModels;
using EWTSS_DESKTOP.Presentation.Views.Login;
using EWTSS_DESKTOP.Presentation.Views;
using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Presentation.Views.Scenario;
using System.Windows.Controls;

namespace EWTSS_DESKTOP
{
    public partial class MainWindow : Window
    {

        private readonly StkEngineService _stkEngineService = new StkEngineService();
        public MainWindow()
        {
            InitializeComponent();

            var db = new AppDbContext();
            var userService = new UserService(new UserRepository(db));

            var loginVM = new LoginViewModel(userService);
            var loginPage = new LoginView { DataContext = loginVM };

            loginVM.LoginSucceeded += (user) =>
            {
                var dashboard = new ScenarioDashboardView(user, _stkEngineService);
                MainFrame.Navigate(dashboard);
            };

            MainFrame.Navigate(loginPage);
        }

        public void NavigateTo(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}