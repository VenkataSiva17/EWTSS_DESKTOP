using System;
using System.Windows.Input;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Commands;

namespace EWTSS_DESKTOP.Presentation.ViewModels
{
    public class LoginViewModel
    {
        private readonly UserService _userService;

        public string Username { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }

        public event Action<User> LoginSucceeded;

        public LoginViewModel(UserService userService)
        {
            _userService = userService;
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            var user = _userService.ValidateUser(Username, Password);

            if (user != null)
            {
                UserSession.CurrentUser = user;
                LoginSucceeded?.Invoke(user);
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid username or password");
            }
        }
    }
}