using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using EWTSS_DESKTOP.Helpers;
using EWTSS_DESKTOP.Infrastructure.Data;
using EWTSS_DESKTOP.Infrastructure.Repositories;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Presentation.ViewModels;

namespace EWTSS_DESKTOP.Presentation.Views.UserManagement
{
    public partial class UserManagementView : Page
    {
        private readonly DispatcherTimer _clockTimer;
        private readonly UserManagementViewModel _viewModel;
        private readonly UserManagementService _userService;
        private bool _isPasswordVisible = false;
        private bool _isConfirmPasswordVisible = false;
        private UserRowViewModel? _selectedUser;

        public UserManagementView()
        {
            InitializeComponent();

            _viewModel = new UserManagementViewModel();
            DataContext = _viewModel;

            var dbContext = new AppDbContext();
            var repo = new UserManagementRepository(dbContext);
            _userService = new UserManagementService(repo, dbContext);

            FirstNameTextBox.GotFocus += RemovePlaceholder;
            LastNameTextBox.GotFocus += RemovePlaceholder;
            UserNameTextBox.GotFocus += RemovePlaceholder;

            FirstNameTextBox.LostFocus += AddPlaceholder;
            LastNameTextBox.LostFocus += AddPlaceholder;
            UserNameTextBox.LostFocus += AddPlaceholder;

            _clockTimer = ClockHelper.StartClock(TimeText);
            Unloaded += UserManagementView_Unloaded;

            LoadUsers();
        }

        private void LoadUsers()
        {
            var users = _userService.GetAllUsers();
            _viewModel.LoadUsers(users);
        }

        private void CreateNew_Click(object sender, RoutedEventArgs e)
        {
            ResetCreateUserForm();
            CreateUserOverlay.Visibility = Visibility.Visible;
        }

        private void CloseCreateUser_Click(object sender, RoutedEventArgs e)
        {
            ResetCreateUserForm();
            CreateUserOverlay.Visibility = Visibility.Collapsed;
        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();
            string userName = UserNameTextBox.Text.Trim();
            string password = _viewModel.IsPasswordVisible
                ? PasswordVisibleTextBox.Text.Trim()
                : PasswordInput.Password.Trim();

            string role = string.Empty;

            if (RoleComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                role = selectedItem.Content?.ToString() ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(firstName) || firstName == _viewModel.FirstNamePlaceholder ||
                string.IsNullOrWhiteSpace(userName) || userName == _viewModel.UserNamePlaceholder ||
                string.IsNullOrWhiteSpace(password))
            {
                System.Windows.MessageBox.Show("Please fill all required fields.");
                return;
            }

            try
            {
                bool isSaved = _userService.CreateUser(firstName, lastName, userName, password, role);

                if (isSaved)
                {
                    System.Windows.MessageBox.Show("User created successfully");
                    ResetCreateUserForm();
                    CreateUserOverlay.Visibility = Visibility.Collapsed;
                    LoadUsers();
                }
                else
                {
                    System.Windows.MessageBox.Show("Failed to create user");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox tb && _viewModel.IsPlaceholderText(tb.Text))
            {
                tb.Text = string.Empty;
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
                EyeClosedSlash.Visibility = Visibility.Collapsed;
                _viewModel.IsPasswordVisible = false;
            }
            else
            {
                PasswordVisibleTextBox.Text = PasswordInput.Password;
                PasswordInput.Visibility = Visibility.Collapsed;
                PasswordVisibleTextBox.Visibility = Visibility.Visible;
                EyeClosedSlash.Visibility = Visibility.Visible;
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

        private void ResetCreateUserForm()
        {
            FirstNameTextBox.Text = _viewModel.FirstNamePlaceholder;
            LastNameTextBox.Text = _viewModel.LastNamePlaceholder;
            UserNameTextBox.Text = _viewModel.UserNamePlaceholder;

            PasswordInput.Password = string.Empty;
            PasswordVisibleTextBox.Text = string.Empty;

            PasswordVisibleTextBox.Visibility = Visibility.Collapsed;
            PasswordInput.Visibility = Visibility.Visible;

            EyeClosedSlash.Visibility = Visibility.Collapsed;
            _viewModel.IsPasswordVisible = false;

            RoleComboBox.SelectedIndex = 0;
        }
     private void UserManagementView_Unloaded(object sender, RoutedEventArgs e)
{
    _clockTimer?.Stop();
}
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.Tag is UserRowViewModel user)
            {
                _selectedUser = user;

                EditUserNameTextBox.Text = user.UserName;
                EditFirstNameTextBox.Text = user.FirstName;
                EditLastNameTextBox.Text = user.LastName;

                foreach (ComboBoxItem item in EditRoleComboBox.Items)
                {
                    string itemText = item.Content?.ToString() ?? string.Empty;
                    if (itemText.ToUpper() == user.RoleName.ToUpper())
                    {
                        EditRoleComboBox.SelectedItem = item;
                        break;
                    }
                }

                UpdateUserOverlay.Visibility = Visibility.Visible;
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.Tag is UserRowViewModel user)
            {
                _selectedUser = user;
                DeleteConfirmationText.Text = $"ARE YOU SURE YOU WANT TO DELETE {user.UserName.ToUpper()} ?";
                DeleteUserOverlay.Visibility = Visibility.Visible;
            }
        }

        private void PermissionUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.Tag is UserRowViewModel user)
            {
                _selectedUser = user;
                ClearPermissionChecks();
                PermissionOverlay.Visibility = Visibility.Visible;
            }
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button && button.Tag is UserRowViewModel user)
            {
                _selectedUser = user;
                ResetPasswordBox.Password = string.Empty;
                ConfirmResetPasswordBox.Password = string.Empty;
                ResetPasswordOverlay.Visibility = Visibility.Visible;
            }
        }

        private void CloseUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            UpdateUserOverlay.Visibility = Visibility.Collapsed;
        }

        private void CloseDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            DeleteUserOverlay.Visibility = Visibility.Collapsed;
        }

        private void ClosePermission_Click(object sender, RoutedEventArgs e)
        {
            PermissionOverlay.Visibility = Visibility.Collapsed;
        }

        private void CloseResetPassword_Click(object sender, RoutedEventArgs e)
{
    ResetPasswordOverlay.Visibility = Visibility.Collapsed;

    ResetPasswordBox.Password = string.Empty;
    ConfirmResetPasswordBox.Password = string.Empty;
    ResetPasswordVisibleTextBox.Text = string.Empty;
    ConfirmResetPasswordVisibleTextBox.Text = string.Empty;

    ResetPasswordVisibleTextBox.Visibility = Visibility.Collapsed;
    ConfirmResetPasswordVisibleTextBox.Visibility = Visibility.Collapsed;
    ResetPasswordBox.Visibility = Visibility.Visible;
    ConfirmResetPasswordBox.Visibility = Visibility.Visible;

    ResetEyeClosedSlash.Visibility = Visibility.Collapsed;
    ConfirmEyeClosedSlash.Visibility = Visibility.Collapsed;

    _isResetPasswordVisible = false;
    _isConfirmResetPasswordVisible = false;
}

        private void SaveUpdatedUser_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                System.Windows.MessageBox.Show("No user selected.");
                return;
            }

            try
            {
                string firstName = EditFirstNameTextBox.Text.Trim();
                string lastName = EditLastNameTextBox.Text.Trim();
                string userName = EditUserNameTextBox.Text.Trim();

                string roleName = string.Empty;
                if (EditRoleComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    roleName = selectedItem.Content?.ToString() ?? string.Empty;
                }

                bool updated = _userService.UpdateUser(_selectedUser.Id, firstName, lastName, userName, roleName);

                if (updated)
                {
                    System.Windows.MessageBox.Show("User updated successfully.");
                    UpdateUserOverlay.Visibility = Visibility.Collapsed;
                    LoadUsers();
                }
                else
                {
                    System.Windows.MessageBox.Show("Failed to update user.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        

        private void ConfirmDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                System.Windows.MessageBox.Show("No user selected.");
                return;
            }

            try
            {
                bool deleted = _userService.DeleteUser(_selectedUser.Id);

                if (deleted)
                {
                    System.Windows.MessageBox.Show("User deleted successfully.");
                    DeleteUserOverlay.Visibility = Visibility.Collapsed;
                    LoadUsers();
                }
                else
                {
                    System.Windows.MessageBox.Show("Failed to delete user.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void SubmitResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                System.Windows.MessageBox.Show("No user selected.");
                return;
            }

            try
            {
                string newPassword = ResetPasswordBox.Password.Trim();
                string confirmPassword = ConfirmResetPasswordBox.Password.Trim();

                if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    System.Windows.MessageBox.Show("Please fill both password fields.");
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    System.Windows.MessageBox.Show("Passwords do not match.");
                    return;
                }

                bool reset = _userService.ResetPassword(_selectedUser.Id, newPassword);

                if (reset)
                {
                    System.Windows.MessageBox.Show("Password reset successfully.");
                    ResetPasswordOverlay.Visibility = Visibility.Collapsed;
                    ResetPasswordBox.Password = string.Empty;
                    ConfirmResetPasswordBox.Password = string.Empty;
                }
                else
                {
                    System.Windows.MessageBox.Show("Failed to reset password.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void SavePermission_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                System.Windows.MessageBox.Show("No user selected.");
                return;
            }

            try
            {
                var featureIds = new List<int>();

                if (PermissionUserCreate.IsChecked == true) featureIds.Add(1);
                if (PermissionUserDelete.IsChecked == true) featureIds.Add(2);
                if (PermissionUserUpdate.IsChecked == true) featureIds.Add(3);
                if (PermissionForgotPassword.IsChecked == true) featureIds.Add(4);
                if (PermissionChangePassword.IsChecked == true) featureIds.Add(5);

                if (PermissionScenarioCreate.IsChecked == true) featureIds.Add(6);
                if (PermissionScenarioDuplicate.IsChecked == true) featureIds.Add(7);
                if (PermissionScenarioUpdate.IsChecked == true) featureIds.Add(8);
                if (PermissionScenarioDelete.IsChecked == true) featureIds.Add(9);

                if (PermissionDbPurge.IsChecked == true) featureIds.Add(10);
                if (PermissionDbImport.IsChecked == true) featureIds.Add(11);
                if (PermissionDbRestore.IsChecked == true) featureIds.Add(12);

                if (PermissionDrsSentLog.IsChecked == true) featureIds.Add(13);
                if (PermissionUserLog.IsChecked == true) featureIds.Add(14);
                if (PermissionDrsReceiveLog.IsChecked == true) featureIds.Add(15);
                if (PermissionSystemLog.IsChecked == true) featureIds.Add(16);

                if (PermissionCreateEmitterLibrary.IsChecked == true) featureIds.Add(17);
                if (PermissionUpdateEmitterLibrary.IsChecked == true) featureIds.Add(18);
                if (PermissionDeleteEmitterLibrary.IsChecked == true) featureIds.Add(19);
                if (PermissionViewEmitterLibrary.IsChecked == true) featureIds.Add(20);

                bool saved = _userService.SaveUserPermissions(_selectedUser.Id, featureIds);

                if (saved)
                {
                    System.Windows.MessageBox.Show("Permissions updated successfully.");
                    PermissionOverlay.Visibility = Visibility.Collapsed;
                    LoadUsers();
                }
                else
                {
                    System.Windows.MessageBox.Show("Failed to update permissions.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void ClearPermissionChecks()
        {
            PermissionUserCreate.IsChecked = false;
            PermissionUserDelete.IsChecked = false;
            PermissionUserUpdate.IsChecked = false;
            PermissionForgotPassword.IsChecked = false;
            PermissionChangePassword.IsChecked = false;

            PermissionScenarioCreate.IsChecked = false;
            PermissionScenarioDuplicate.IsChecked = false;
            PermissionScenarioUpdate.IsChecked = false;
            PermissionScenarioDelete.IsChecked = false;

            PermissionDbPurge.IsChecked = false;
            PermissionDbImport.IsChecked = false;
            PermissionDbRestore.IsChecked = false;

            PermissionDrsSentLog.IsChecked = false;
            PermissionUserLog.IsChecked = false;
            PermissionDrsReceiveLog.IsChecked = false;
            PermissionSystemLog.IsChecked = false;

            PermissionCreateEmitterLibrary.IsChecked = false;
            PermissionUpdateEmitterLibrary.IsChecked = false;
            PermissionDeleteEmitterLibrary.IsChecked = false;
            PermissionViewEmitterLibrary.IsChecked = false;
        }
private bool _isResetPasswordVisible = false;
private bool _isConfirmResetPasswordVisible = false;

private void TogglePassword_Click(object sender, RoutedEventArgs e)
{
    if (_isResetPasswordVisible)
    {
        ResetPasswordBox.Password = ResetPasswordVisibleTextBox.Text;
        ResetPasswordVisibleTextBox.Visibility = Visibility.Collapsed;
        ResetPasswordBox.Visibility = Visibility.Visible;
        ResetEyeClosedSlash.Visibility = Visibility.Collapsed;
        _isResetPasswordVisible = false;
    }
    else
    {
        ResetPasswordVisibleTextBox.Text = ResetPasswordBox.Password;
        ResetPasswordBox.Visibility = Visibility.Collapsed;
        ResetPasswordVisibleTextBox.Visibility = Visibility.Visible;
        ResetEyeClosedSlash.Visibility = Visibility.Visible;
        _isResetPasswordVisible = true;
    }
}

private void ToggleConfirmPassword_Click(object sender, RoutedEventArgs e)
{
    if (_isConfirmResetPasswordVisible)
    {
        ConfirmResetPasswordBox.Password = ConfirmResetPasswordVisibleTextBox.Text;
        ConfirmResetPasswordVisibleTextBox.Visibility = Visibility.Collapsed;
        ConfirmResetPasswordBox.Visibility = Visibility.Visible;
        ConfirmEyeClosedSlash.Visibility = Visibility.Collapsed;
        _isConfirmResetPasswordVisible = false;
    }
    else
    {
        ConfirmResetPasswordVisibleTextBox.Text = ConfirmResetPasswordBox.Password;
        ConfirmResetPasswordBox.Visibility = Visibility.Collapsed;
        ConfirmResetPasswordVisibleTextBox.Visibility = Visibility.Visible;
        ConfirmEyeClosedSlash.Visibility = Visibility.Visible;
        _isConfirmResetPasswordVisible = true;
    }
}

private void ResetPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
{
    if (!_isResetPasswordVisible)
    {
        ResetPasswordVisibleTextBox.Text = ResetPasswordBox.Password;
    }
}

private void ResetPasswordVisibleTextBox_TextChanged(object sender, TextChangedEventArgs e)
{
    if (_isResetPasswordVisible)
    {
        ResetPasswordBox.Password = ResetPasswordVisibleTextBox.Text;
    }
}

private void ConfirmResetPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
{
    if (!_isConfirmResetPasswordVisible)
    {
        ConfirmResetPasswordVisibleTextBox.Text = ConfirmResetPasswordBox.Password;
    }
}

private void ConfirmResetPasswordVisibleTextBox_TextChanged(object sender, TextChangedEventArgs e)
{
    if (_isConfirmResetPasswordVisible)
    {
        ConfirmResetPasswordBox.Password = ConfirmResetPasswordVisibleTextBox.Text;
    }
}
    }
}