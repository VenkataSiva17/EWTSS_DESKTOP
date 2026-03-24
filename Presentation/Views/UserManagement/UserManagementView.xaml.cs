using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using EWTSS_DESKTOP.Helpers;

namespace EWTSS_DESKTOP.Presentation.Views.UserManagement
{
    public partial class UserManagementView : Page
    {
        private bool _isPasswordVisible = false;
        private readonly DispatcherTimer _clockTimer;

        public UserManagementView()
        {
            InitializeComponent();

            FirstNameTextBox.GotFocus += RemovePlaceholder;
            LastNameTextBox.GotFocus += RemovePlaceholder;
            UserNameTextBox.GotFocus += RemovePlaceholder;

            FirstNameTextBox.LostFocus += AddPlaceholder;
            LastNameTextBox.LostFocus += AddPlaceholder;
            UserNameTextBox.LostFocus += AddPlaceholder;

            _clockTimer = ClockHelper.StartClock(TimeText);
        }

        private void CreateNew_Click(object sender, RoutedEventArgs e)
        {
            CreateUserOverlay.Visibility = Visibility.Visible;
        }

        private void CloseCreateUser_Click(object sender, RoutedEventArgs e)
        {
            CreateUserOverlay.Visibility = Visibility.Collapsed;
        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("User created successfully");

            FirstNameTextBox.Text = "Enter your first name";
            LastNameTextBox.Text = "Enter your last name";
            UserNameTextBox.Text = "Enter your user name";
            PasswordInput.Password = "";
            PasswordVisibleTextBox.Text = "";
            PasswordVisibleTextBox.Visibility = Visibility.Collapsed;
            PasswordInput.Visibility = Visibility.Visible;
            TogglePasswordButton.Content = "\uE722";
            _isPasswordVisible = false;
            RoleComboBox.SelectedIndex = 0;

            CreateUserOverlay.Visibility = Visibility.Collapsed;
        }

        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                if (tb.Text == "Enter your first name" ||
                    tb.Text == "Enter your last name" ||
                    tb.Text == "Enter your user name")
                {
                    tb.Text = "";
                }
            }
        }

        private void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                if (tb.Name == "FirstNameTextBox")
                    tb.Text = "Enter your first name";
                else if (tb.Name == "LastNameTextBox")
                    tb.Text = "Enter your last name";
                else if (tb.Name == "UserNameTextBox")
                    tb.Text = "Enter your user name";
            }
        }

        private void TogglePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPasswordVisible)
            {
                PasswordInput.Password = PasswordVisibleTextBox.Text;
                PasswordVisibleTextBox.Visibility = Visibility.Collapsed;
                PasswordInput.Visibility = Visibility.Visible;
                TogglePasswordButton.Content = "\uE722";
                _isPasswordVisible = false;
            }
            else
            {
                PasswordVisibleTextBox.Text = PasswordInput.Password;
                PasswordInput.Visibility = Visibility.Collapsed;
                PasswordVisibleTextBox.Visibility = Visibility.Visible;
                TogglePasswordButton.Content = "\uE711";
                _isPasswordVisible = true;
            }
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!_isPasswordVisible)
            {
                PasswordVisibleTextBox.Text = PasswordInput.Password;
            }
        }

        private void PasswordVisibleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isPasswordVisible)
            {
                PasswordInput.Password = PasswordVisibleTextBox.Text;
            }
        }
    }
}