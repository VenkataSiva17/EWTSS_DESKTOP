using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

using EWTSS_DESKTOP.Infrastructure.Data;
using EWTSS_DESKTOP.Infrastructure.Services;
using EWTSS_DESKTOP.Core.Models;
using ScenarioModel = EWTSS_DESKTOP.Core.Models.Scenario;

using MessageBox = System.Windows.MessageBox;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioEditorPage : Page
    {
        private readonly int _scenarioId;
        private readonly StkEngineService _stkEngineService;

        private Stk3DViewControl _stk3DView;
        private Stk2DViewControl _stk2DView;

        public ScenarioEditorPage(int scenarioId, StkEngineService stkEngineService)
        {
            InitializeComponent();

            _scenarioId = scenarioId;
            _stkEngineService = stkEngineService;

            ScenarioDescriptionTextBox.TextChanged += ScenarioDescriptionTextBox_TextChanged;

            LoadScenario();
            LoadTablePreview();
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

            ScenarioTitleTextBlock.Text = $"{scenario.Name} | - TREE STRUCTURE";
            UserNameTextBlock.Text = scenario.User != null
                ? $"{scenario.User.FirstName} {scenario.User.LastName}"
                : "Admin Admin";

            ScenarioNameTextBox.Text = scenario.Name;
            ScenarioDescriptionTextBox.Text = scenario.Description ?? "";
            ScenarioCreatedDateTextBlock.Text = $"SCENARIO CREATION DATE(DD:MM:YYYY) : {scenario.StartDate:dd-MM-yyyy}";
            ScenarioCreatedTimeTextBlock.Text = $"SCENARIO CREATION TIME(HH:MM:SS) : {scenario.StartTime}";
            ScenarioDurationTextBox.Text = scenario.Duration.ToString();
            ScenarioExecutionTimeTextBox.Text = scenario.ExecuteTime?.ToString() ?? "--:--:--";

            UpdateDescriptionCount();
            LoadScenarioTree(scenario);
            InitializeStkViews(scenario.Name);
        }

        private void InitializeStkViews(string scenarioName)
        {
            try
            {
                _stkEngineService.Initialize();

                _stk3DView = new Stk3DViewControl();
                _stk2DView = new Stk2DViewControl();

                Host3D.Child = _stk3DView;
                Host2D.Child = _stk2DView;

                _stk3DView.CreateScenario(scenarioName);
                _stk2DView.CreateScenario(scenarioName);

                Show3D();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"STK visualization could not be initialized.\n\n{ex.Message}",
                    "STK Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Host3D.Visibility = Visibility.Collapsed;
                Host2D.Visibility = Visibility.Collapsed;

                Placeholder3DText.Visibility = Visibility.Visible;
                Placeholder2DText.Visibility = Visibility.Collapsed;
            }
        }

        private void Show3D()
        {
            Host3D.Visibility = Visibility.Visible;
            Host2D.Visibility = Visibility.Collapsed;

            Placeholder3DText.Visibility = Visibility.Collapsed;
            Placeholder2DText.Visibility = Visibility.Collapsed;

            Btn3D.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter()
                .ConvertFromString("#062235");
            Btn2D.Background = System.Windows.Media.Brushes.Transparent;
        }

        private void Show2D()
        {
            Host3D.Visibility = Visibility.Collapsed;
            Host2D.Visibility = Visibility.Visible;

            Placeholder3DText.Visibility = Visibility.Collapsed;
            Placeholder2DText.Visibility = Visibility.Collapsed;

            Btn2D.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter()
                .ConvertFromString("#062235");
            Btn3D.Background = System.Windows.Media.Brushes.Transparent;
        }

        private void Btn3D_Click(object sender, RoutedEventArgs e)
        {
            Show3D();
        }

        private void Btn2D_Click(object sender, RoutedEventArgs e)
        {
            Show2D();
        }

        private void LoadScenarioTree(ScenarioModel scenario)
        {
            ScenarioTree.Items.Clear();

            var root = CreateTreeNode(scenario.Name, true, true);

            root.Items.Add(CreateTreeNode("AREA OF OPERATION"));

            var blueLine = CreateTreeNode("BLUE LINE");
            var blueCc = CreateTreeNode("CC");
            blueCc.Items.Add(CreateTreeNode("CC1"));
            blueLine.Items.Add(blueCc);

            var alliedCom = CreateTreeNode("COMMEMITTERALLIED");
            alliedCom.Items.Add(CreateTreeNode("COMMEMITTERALLIED1"));
            alliedCom.Items.Add(CreateTreeNode("COMMEMITTERALLIED2"));
            blueLine.Items.Add(alliedCom);

            blueLine.Items.Add(CreateTreeNode("RADAREMITTERALLIED"));
            root.Items.Add(blueLine);

            var redLine = CreateTreeNode("RED LINE");
            redLine.Items.Add(CreateTreeNode("NET"));

            var commEmitter = CreateTreeNode("COMEMITTER");
            commEmitter.Items.Add(CreateTreeNode("COMEMITTER1"));
            commEmitter.Items.Add(CreateTreeNode("COMEMITTER2"));
            commEmitter.Items.Add(CreateTreeNode("COMEMITTER3"));
            commEmitter.Items.Add(CreateTreeNode("COMEMITTER4"));
            redLine.Items.Add(commEmitter);

            redLine.Items.Add(CreateTreeNode("RADAR"));
            root.Items.Add(redLine);

            ScenarioTree.Items.Add(root);
        }

        private TreeViewItem CreateTreeNode(string text, bool isExpanded = false, bool cyan = false)
        {
            return new TreeViewItem
            {
                Header = new TextBlock
                {
                    Text = text,
                    Foreground = cyan
                        ? new System.Windows.Media.SolidColorBrush(
                            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#21B3AE"))
                        : System.Windows.Media.Brushes.White,
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 13
                },
                IsExpanded = isExpanded
            };
        }

        private void LoadTablePreview()
        {
            var rows = new List<DrsPreviewRow>
            {
                new() { Sno = 1, Side = "BLUE", EmitterName = "COMMEMITTERALLIED1", EmitterType = "CUSTOMIZED", Mode = "FF", Modulation = "AM" },
                new() { Sno = 2, Side = "BLUE", EmitterName = "COMMEMITTERALLIED2", EmitterType = "CUSTOMIZED", Mode = "FH", Modulation = "" },
                new() { Sno = 3, Side = "RED", EmitterName = "COMEMITTER1", EmitterType = "CUSTOMIZED", Mode = "FF", Modulation = "AM" },
                new() { Sno = 4, Side = "RED", EmitterName = "COMEMITTER2", EmitterType = "CUSTOMIZED", Mode = "FH", Modulation = "" },
                new() { Sno = 5, Side = "RED", EmitterName = "COMEMITTER3", EmitterType = "CUSTOMIZED", Mode = "BURST", Modulation = "AM" }
            };

            DrsDataGrid.ItemsSource = rows;
        }

        private void ScenarioDescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDescriptionCount();
        }

        private void UpdateDescriptionCount()
        {
            int remaining = Math.Max(0, 256 - (ScenarioDescriptionTextBox.Text?.Length ?? 0));
            DescriptionCountTextBlock.Text = $"{remaining} characters remaining";
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

            ScenarioTitleTextBlock.Text = $"{scenario.Name} | - TREE STRUCTURE";
            MessageBox.Show("Scenario updated successfully.");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }

    public class DrsPreviewRow
    {
        public int Sno { get; set; }
        public string Side { get; set; }
        public string EmitterName { get; set; }
        public string EmitterType { get; set; }
        public string Mode { get; set; }
        public string Modulation { get; set; }
    }
}