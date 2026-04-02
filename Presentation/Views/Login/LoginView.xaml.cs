using System.Windows;
using System.Windows.Controls;
using EWTSS_DESKTOP.Presentation.ViewModels;

namespace EWTSS_DESKTOP.Presentation.Views.Login
{
    public partial class LoginView : Page
    {
        private bool _isPasswordVisible = false;
        private bool _isInternalUpdate = false;

        public LoginView()
        {
            InitializeComponent();
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
                BtnTogglePassword.Content = "\uE8F5"; // hide icon
            }
            else
            {
                PasswordBox.Password = TextBoxVisible.Text;
                PasswordBox.Visibility = Visibility.Visible;
                TextBoxVisible.Visibility = Visibility.Collapsed;
                BtnTogglePassword.Content = "\uE722"; // view icon
            }

            _isInternalUpdate = false;
        }
    }
}