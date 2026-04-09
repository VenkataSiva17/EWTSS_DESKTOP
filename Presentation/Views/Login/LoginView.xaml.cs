using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Infrastructure.Data;
using EWTSS_DESKTOP.Infrastructure.Repositories;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Presentation.ViewModels;
using EWTSS_DESKTOP.Presentation.Views.Dashboard;
using System.Windows;
using System.Windows.Controls;

namespace EWTSS_DESKTOP.Presentation.Views.Login
{
    public partial class LoginView : Page
    {
        private bool _isPasswordVisible = false;
        private bool _isInternalUpdate = false;
        private readonly LoginViewModel _viewModel;

        public LoginView()
        {
            InitializeComponent();

            var dbContext = new AppDbContext();
            var userRepository = new UserRepository(dbContext);
            var userService = new UserService(userRepository);

            _viewModel = new LoginViewModel(userService);
            _viewModel.LoginSucceeded += OnLoginSucceeded;

            DataContext = _viewModel;
        }

        private void OnLoginSucceeded(User user)
        {
            var stkEngineService = new StkEngineService();

            if (System.Windows.Application.Current.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
            {
                mainWindow.MainFrame.Navigate(new ScenarioDashboardView(user, stkEngineService));
            }
            else
            {
                NavigationService?.Navigate(new ScenarioDashboardView(user, stkEngineService));
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_isInternalUpdate)
                return;

            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }

            if (_isPasswordVisible)
            {
                _isInternalUpdate = true;
                TextBoxVisible.Text = PasswordBox.Password;
                _isInternalUpdate = false;
            }
        }

        private void TextBoxVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isInternalUpdate || !_isPasswordVisible)
                return;

            _isInternalUpdate = true;

            PasswordBox.Password = TextBoxVisible.Text;

            if (DataContext is LoginViewModel vm)
            {
                vm.Password = TextBoxVisible.Text;
            }

            _isInternalUpdate = false;
        }

        private void BtnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;

            _isInternalUpdate = true;

            if (_isPasswordVisible)
            {
                TextBoxVisible.Text = PasswordBox.Password;
                TextBoxVisible.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                BtnTogglePassword.Content = "\uE8F5";
            }
            else
            {
                PasswordBox.Password = TextBoxVisible.Text;
                PasswordBox.Visibility = Visibility.Visible;
                TextBoxVisible.Visibility = Visibility.Collapsed;
                BtnTogglePassword.Content = "\uE722";
            }

            _isInternalUpdate = false;
        }
    }
}