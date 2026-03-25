using System;
using System.Windows;
using System.Windows.Controls;
using EWTSS_DESKTOP.Infrastructure.Repositories;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Infrastructure.Data;
using EWTSS_DESKTOP.Presentation.ViewModels;
using EWTSS_DESKTOP.Presentation.Views.Login;
using EWTSS_DESKTOP.Presentation.Views.Dashboard;

namespace EWTSS_DESKTOP
{
    public partial class MainWindow : Window
    {
        private readonly StkEngineService _stkEngineService;

        public MainWindow()
        {
            InitializeComponent();

            Console.WriteLine("App started");

            // ✅ Create SINGLE instance
            _stkEngineService = new StkEngineService();

            // ✅ Check license FIRST
            if (!_stkEngineService.CheckLicense())
            {
                Application.Current.Shutdown();
                return;
            }

            // DB + Services
            var db = new AppDbContext();
            var userService = new UserService(new UserRepository(db));

            // Login setup
            var loginVM = new LoginViewModel(userService);
            var loginPage = new LoginView { DataContext = loginVM };

            // ✅ Navigate after login
            loginVM.LoginSucceeded += (user) =>
            {
                var dashboard = new ScenarioDashboardView(user);

                // OPTIONAL: pass STK service to dashboard if needed
                // var dashboard = new ScenarioDashboardView(user, _stkEngineService);

                MainFrame.Navigate(dashboard);
            };

            MainFrame.Navigate(loginPage);
        }

        public void NavigateTo(Page page)
        {
            MainFrame.Navigate(page);
        }

        // ✅ Clean up STK when app closes
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _stkEngineService?.Dispose();
        }
    }
}