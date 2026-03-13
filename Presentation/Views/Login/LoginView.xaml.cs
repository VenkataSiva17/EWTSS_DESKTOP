using System.Windows.Controls;
using EWTSS_DESKTOP.Presentation.ViewModels;

namespace EWTSS_DESKTOP.Presentation.Views.Login
{
    public partial class LoginView : Page
    {
        private bool _isPasswordVisible = false;

        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }

            // Sync TextBox if password is visible
            if (_isPasswordVisible)
            {
                TextBoxVisible.Text = PasswordBox.Password;
            }
        }

        private void BtnTogglePassword_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;

            if (_isPasswordVisible)
            {
                // Show password
                TextBoxVisible.Text = PasswordBox.Password;
                TextBoxVisible.Visibility = System.Windows.Visibility.Visible;
                PasswordBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                // Hide password
                PasswordBox.Password = TextBoxVisible.Text;
                PasswordBox.Visibility = System.Windows.Visibility.Visible;
                TextBoxVisible.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}