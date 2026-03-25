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
        private readonly AppDbContext _db;

        public StkEngineService StkEngineService => _stkEngineService;

        public MainWindow()
        {
            InitializeComponent();

            WindowState = WindowState.Maximized;

            _stkEngineService = new StkEngineService();

            if (!_stkEngineService.CheckLicense())
            {
                System.Windows.Application.Current.Shutdown();
                return;
            }

            _db = new AppDbContext();
            var userService = new UserService(new UserRepository(_db));

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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _stkEngineService?.Dispose();
            _db?.Dispose();
        }
    }
}