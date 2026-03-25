using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using EWTSS_DESKTOP.Helpers;
using EWTSS_DESKTOP.Presentation.ViewModels;

namespace EWTSS_DESKTOP.Presentation.Views.UserManagement
{
    public partial class UserManagementView : Page
    {
        private readonly DispatcherTimer _clockTimer;
        private readonly UserManagementViewModel _viewModel;

        public UserManagementView()
        {
            InitializeComponent();

            _viewModel = new UserManagementViewModel();
            DataContext = _viewModel;

            FirstNameTextBox.GotFocus += RemovePlaceholder;
            LastNameTextBox.GotFocus += RemovePlaceholder;
            UserNameTextBox.GotFocus += RemovePlaceholder;

            FirstNameTextBox.LostFocus += AddPlaceholder;
            LastNameTextBox.LostFocus += AddPlaceholder;
            UserNameTextBox.LostFocus += AddPlaceholder;

            _clockTimer = ClockHelper.StartClock(TimeText);
            this.Unloaded += UserManagementView_Unloaded; 
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
            System.Windows.Forms.MessageBox.Show("User created successfully");

            FirstNameTextBox.Text = _viewModel.FirstNamePlaceholder;
            LastNameTextBox.Text = _viewModel.LastNamePlaceholder;
            UserNameTextBox.Text = _viewModel.UserNamePlaceholder;
            PasswordInput.Password = "";
            PasswordVisibleTextBox.Text = "";
            PasswordVisibleTextBox.Visibility = Visibility.Collapsed;
            PasswordInput.Visibility = Visibility.Visible;
            TogglePasswordButton.Content = _viewModel.GetShowPasswordIcon();
            _viewModel.IsPasswordVisible = false;
            RoleComboBox.SelectedIndex = 0;

            CreateUserOverlay.Visibility = Visibility.Collapsed;
        }

        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tb)
            {
                if (_viewModel.IsPlaceholderText(tb.Text))
                {
                    tb.Text = "";
                }
            }
        }

        private void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = _viewModel.GetPlaceholderByName(tb.Name);
            }
        }

        private void TogglePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.IsPasswordVisible)
            {
                PasswordInput.Password = PasswordVisibleTextBox.Text;
                PasswordVisibleTextBox.Visibility = Visibility.Collapsed;
                PasswordInput.Visibility = Visibility.Visible;
                TogglePasswordButton.Content = _viewModel.GetShowPasswordIcon();
                _viewModel.IsPasswordVisible = false;
            }
            else
            {
                PasswordVisibleTextBox.Text = PasswordInput.Password;
                PasswordInput.Visibility = Visibility.Collapsed;
                PasswordVisibleTextBox.Visibility = Visibility.Visible;
                TogglePasswordButton.Content = _viewModel.GetHidePasswordIcon();
                _viewModel.IsPasswordVisible = true;
            }
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.IsPasswordVisible)
            {
                PasswordVisibleTextBox.Text = PasswordInput.Password;
            }
        }

        private void PasswordVisibleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_viewModel.IsPasswordVisible)
            {
                PasswordInput.Password = PasswordVisibleTextBox.Text;
            }
        }

        private void UserManagementView_Unloaded(object sender, RoutedEventArgs e)
        {
            _clockTimer.Stop();
        }
    }
}