using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EWTSS_DESKTOP.Core.Models;

namespace EWTSS_DESKTOP.Presentation.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private User _currentUser;

        public DashboardViewModel(User user)
        {
            _currentUser = user;

            // Example: Initialize dashboard data
            WelcomeMessage = $"Welcome, {_currentUser.username}!";
            // You can add more initialization, e.g., load charts, lists, etc.
        }

        #region Properties

        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged();
            }
        }

        // Example: a list of dashboard items
        private ObservableCollection<string> _dashboardItems = new();
        public ObservableCollection<string> DashboardItems
        {
            get => _dashboardItems;
            set
            {
                _dashboardItems = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        // Example: You can add commands like LogoutCommand, RefreshCommand etc.
        // public ICommand LogoutCommand { get; set; }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}