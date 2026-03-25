using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using EWTSS_DESKTOP.Commands;
using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Presentation.Views.Scenario;
using EWTSS_DESKTOP.Infrastructure.Data;
using EWTSS_DESKTOP.Infrastructure.Services;
using System;
using System.Windows;

namespace EWTSS_DESKTOP.Presentation.ViewModels
{
    public class ScenarioDashboardViewModel : INotifyPropertyChanged
    {
        private readonly User _loggedInUser;


        private readonly StkEngineService _stkEngineService;

        public ObservableCollection<Scenario> AllScenarios { get; set; } = new();
        public ObservableCollection<Scenario> FilteredScenarios { get; set; } = new();

        public ObservableCollection<string> StatusOptions { get; set; } = new()
        {
            "ALL SCENARIOS",
            "PLANNED",
            "COMPLETED",
            "INPROGRESS"
        };

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private string _selectedStatus = "ALL SCENARIOS";
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private string _loggedInUserName;
        public string LoggedInUserName
        {
            get => _loggedInUserName;
            set
            {
                _loggedInUserName = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand NewScenarioCommand { get; }

        public ICommand DuplicateScenarioCommand { get; }
        public ICommand EditScenarioCommand { get; }
        public ICommand DeleteScenarioCommand { get; }

        public ScenarioDashboardViewModel(User user, StkEngineService stkEngineService)
        {
            _loggedInUser = user;
            _stkEngineService = stkEngineService;

            LoggedInUserName = $"{user.FirstName} {user.LastName}";
            SearchCommand = new RelayCommand(() => ApplyFilters());
            NewScenarioCommand = new RelayCommand(() => OpenNewScenario());
            DuplicateScenarioCommand = new RelayCommand(DuplicateScenario);
            EditScenarioCommand = new RelayCommand(EditScenario);
            DeleteScenarioCommand = new RelayCommand(DeleteScenario);

            LoadScenarios();
        }

        private void LoadScenarios()
        {
            using var db = new AppDbContext();

            var scenarios = db.Scenarios
                .Include(x => x.User)
                .Where(x => x.UserId == _loggedInUser.Id)
                .OrderByDescending(x => x.StartDate)
                .ThenByDescending(x => x.StartTime)
                .ToList();

            AllScenarios.Clear();
            foreach (var item in scenarios)
                AllScenarios.Add(item);

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var query = AllScenarios.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var text = SearchText.Trim().ToLower();

                query = query.Where(x =>
                    (!string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(text)) ||
                    x.ScenarioType.ToString().ToLower().Contains(text) ||
                    x.ScenarioStatus.ToString().ToLower().Contains(text));
            }

            if (!string.IsNullOrWhiteSpace(SelectedStatus) && SelectedStatus != "ALL SCENARIOS")
            {
                query = query.Where(x => x.ScenarioStatus.ToString() == SelectedStatus);
            }

            FilteredScenarios.Clear();
            foreach (var item in query)
                FilteredScenarios.Add(item);
        }

        private void OpenNewScenario()
        {
            var dialog = new CreateScenarioWindow
            {
                Owner =  System.Windows.Application.Current.MainWindow
            };

            var result = dialog.ShowDialog();

            if (result == true)
            {
                using var db = new AppDbContext();

                var scenario = new Scenario
                {
                    Name = dialog.ScenarioName,
                    Description = dialog.ScenarioDescription,
                    StartDate = DateTime.Now,
                    StartTime = DateTime.Now.TimeOfDay,
                    Duration = TimeSpan.Zero,
                    ScenarioType = ScenarioType.ORIGINAL,
                    ScenarioStatus = ScenarioStatus.PLANNED,
                    UserId = _loggedInUser.Id,
                    ExecuteDate = DateTime.Now,
                    ExecuteTime = DateTime.Now.TimeOfDay,
                    ExecuteRun = ExecuteRun.EXECUTE,
                    StartStop = StartStop.START
                };

                db.Scenarios.Add(scenario);
                db.SaveChanges();
                // Console.Write("dfjghdkfhgjkdfhg");

               
                // stkEngineService.CreateScenario(scenario.Name, DateTime.Now, TimeSpan.FromMinutes(20));
                

                LoadScenarios();
            }
        }

        private void DuplicateScenario(object parameter)
        {
            if (parameter is not Scenario sourceScenario)
                return;

            using var db = new AppDbContext();

            var duplicate = new Scenario
            {
                Name = $"{sourceScenario.Name}_copy1",
                Description = sourceScenario.Description,
                UserId = _loggedInUser.Id,
                ScenarioType = ScenarioType.DUPLICATE,
                ScenarioStatus = ScenarioStatus.PLANNED,
                StartDate = DateTime.Now,
                StartTime = DateTime.Now.TimeOfDay,
                ExecuteDate = DateTime.Now,
                ExecuteTime = DateTime.Now.TimeOfDay,
                ExecuteRun = ExecuteRun.EXECUTE,
                StartStop = StartStop.START
            };

            db.Scenarios.Add(duplicate);
            db.SaveChanges();
            


            LoadScenarios();
        }

        private void DeleteScenario(object parameter)
        {
            if (parameter is not Scenario scenario)
                return;

            var result =  System.Windows.MessageBox.Show(
                $"Delete scenario '{scenario.Name}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            using var db = new AppDbContext();

            var dbScenario = db.Scenarios.FirstOrDefault(x => x.Id == scenario.Id);
            if (dbScenario != null)
            {
                db.Scenarios.Remove(dbScenario);
                db.SaveChanges();
            }

            LoadScenarios();
        }

        private void EditScenario(object parameter)
        {
            if (parameter is not Scenario scenario)
                return;

            NavigateToEditor(scenario.Id);
        }

        private void NavigateToEditor(int scenarioId)
        {
            if ( System.Windows.Application.Current.MainWindow is MainWindow mainWindow)
            {
                var editorPage = new ScenarioEditorPage(scenarioId, _stkEngineService);
                mainWindow.NavigateTo(editorPage);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}