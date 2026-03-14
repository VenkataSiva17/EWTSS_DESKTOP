using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EWTSS_DESKTOP.Infrastructure.Data;
using EWTSS_DESKTOP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioEditorPage : Page
    {
        private readonly int _scenarioId;
        private readonly StkEngineService _stkEngineService;

        public ScenarioEditorPage(int scenarioId, StkEngineService stkEngineService)
        {
            InitializeComponent();

            _scenarioId = scenarioId;
            _stkEngineService = stkEngineService;

            LoadScenario();
        }

        private void LoadScenario()
        {
            using var db = new AppDbContext();

            var scenario = db.Scenarios
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == _scenarioId);

            if (scenario == null)
            {
                MessageBox.Show("Scenario not found.");
                return;
            }

            ScenarioNameTextBox.Text = scenario.Name;
            ScenarioDescriptionTextBox.Text = scenario.Description;

            ScenarioTree.Items.Clear();

            var root = new TreeViewItem
            {
                Header = scenario.Name,
                IsExpanded = true
            };

            root.Items.Add(new TreeViewItem { Header = "AREA OF OPERATION" });
            root.Items.Add(new TreeViewItem { Header = "BLUE LINE" });
            root.Items.Add(new TreeViewItem { Header = "RED LINE" });

            ScenarioTree.Items.Add(root);
        }

        private void UpdateScenario_Click(object sender, RoutedEventArgs e)
        {
            using var db = new AppDbContext();

            var scenario = db.Scenarios.FirstOrDefault(x => x.Id == _scenarioId);
            if (scenario == null)
            {
                MessageBox.Show("Scenario not found.");
                return;
            }

            scenario.Name = ScenarioNameTextBox.Text?.Trim();
            scenario.Description = ScenarioDescriptionTextBox.Text?.Trim();

            db.SaveChanges();

            MessageBox.Show("Scenario updated successfully.");
        }
    }
}