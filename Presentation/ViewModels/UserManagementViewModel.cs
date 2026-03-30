using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EWTSS_DESKTOP.Core.Models;

namespace EWTSS_DESKTOP.Presentation.ViewModels
{
    public class UserManagementViewModel : BaseViewModel
    {
        private bool _isPasswordVisible;

        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                _isPasswordVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isConfirmPasswordVisible;

        public bool IsConfirmPasswordVisible
        {
            get => _isConfirmPasswordVisible;
            set
            {
                _isConfirmPasswordVisible = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UserRowViewModel> Users { get; set; }
            = new ObservableCollection<UserRowViewModel>();

        public string FirstNamePlaceholder => "Enter your first name";
        public string LastNamePlaceholder => "Enter your last name";
        public string UserNamePlaceholder => "Enter your user name";

        public string GetPlaceholderByName(string controlName)
        {
            return controlName switch
            {
                "FirstNameTextBox" => FirstNamePlaceholder,
                "LastNameTextBox" => LastNamePlaceholder,
                "UserNameTextBox" => UserNamePlaceholder,
                _ => string.Empty
            };
        }

        public bool IsPlaceholderText(string text)
        {
            return text == FirstNamePlaceholder ||
                   text == LastNamePlaceholder ||
                   text == UserNamePlaceholder;
        }

        public void LoadUsers(List<User> users)
        {
            Users.Clear();

            int sno = 1;

            foreach (var user in users)
            {
                Users.Add(new UserRowViewModel
                {
                    Id = user.Id,
                    SerialNo = sno++,
                    FirstName = user.FirstName ?? string.Empty,
                    LastName = user.LastName ?? string.Empty,
                    FullName = GetFullName(user),
                    UserName = user.UserName ?? string.Empty,
                    RoleName = user.Role?.Name?.ToUpper() ?? "--",
                    Permissions = GetPermissionsText(user),
                    CreatedOnText = user.CreatedOn.ToString("dd-MM-yyyy")
                });
            }
        }

        private string GetFullName(User user)
        {
            string firstName = user.FirstName ?? string.Empty;
            string lastName = user.LastName ?? string.Empty;
            string fullName = $"{firstName} {lastName}".Trim();

            return string.IsNullOrWhiteSpace(fullName) ? "--" : fullName;
        }

        private string GetPermissionsText(User user)
        {
            if (user.Role?.Permissions != null && user.Role.Permissions.Any())
            {
                var permissionNames = user.Role.Permissions
                    .Where(p => p.Feature != null && !string.IsNullOrWhiteSpace(p.Feature.Name))
                    .Select(p => p.Feature!.Name)
                    .Distinct()
                    .ToList();

                if (permissionNames.Count > 0)
                {
                    return string.Join(", ", permissionNames);
                }
            }

            return "--";
        }
    }

    public class UserRowViewModel
    {
        public int Id { get; set; }
        public int SerialNo { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string Permissions { get; set; } = "--";
        public string CreatedOnText { get; set; } = string.Empty;
    }
}