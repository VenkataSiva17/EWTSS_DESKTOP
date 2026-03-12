// LoginViewModel.cs
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EWTSS_DESKTOP.Models;                    // for User
using EWTSS_DESKTOP.Infrastructure.Services;  // for UserService
using EWTSS_DESKTOP.Commands;                 // for RelayCommand
using EWTSS_DESKTOP.Presentation.ViewModels;  // for BaseViewModel

namespace EWTSS_DESKTOP.Presentation.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly UserService _userService;

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public ICommand LoginCommand { get; }

        public event Action<User>? LoginSucceeded;

        public LoginViewModel(UserService userService)
        {
            _userService = userService;
            LoginCommand = new RelayCommand(async _ => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            var user = await _userService.AuthenticateAsync(UserName, Password);
            if (user != null)
            {
                LoginSucceeded?.Invoke(user);
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "Invalid username or password",
                    "Login Failed",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning
                );
            }
        }
    }
}