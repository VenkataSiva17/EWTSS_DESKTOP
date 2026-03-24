using EWTSS_DESKTOP.Presentation.ViewModels;

namespace EWTSS_DESKTOP.Presentation.ViewModels
{
    public class LogManagementViewModel : BaseViewModel
    {
        private string _activeTab;
        private string _searchText;

        public string ActiveTab
        {
            get => _activeTab;
            set
            {
                _activeTab = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSearchPlaceholderVisible));
            }
        }

        public bool IsSearchPlaceholderVisible =>
            string.IsNullOrWhiteSpace(SearchText);

        public LogManagementViewModel()
        {
            ActiveTab = "USER";
            SearchText = string.Empty;
        }

        public void SetUserTab()
        {
            ActiveTab = "USER";
        }

        public void SetSentToDrsTab()
        {
            ActiveTab = "SENT_TO_DRS";
        }

        public void SetReceivedFromDrsTab()
        {
            ActiveTab = "RECEIVED_FROM_DRS";
        }

        public void SetSystemTab()
        {
            ActiveTab = "SYSTEM";
        }
    }
}