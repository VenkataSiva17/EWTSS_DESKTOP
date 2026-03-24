using EWTSS_DESKTOP.Presentation.ViewModels;

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

        public string GetShowPasswordIcon()
        {
            return "\uE722";
        }

        public string GetHidePasswordIcon()
        {
            return "\uE711";
        }
    }
}