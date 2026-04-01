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

using WpfBrush = System.Windows.Media.Brush;
using WpfBrushes = System.Windows.Media.Brushes;
using WpfBrushConverter = System.Windows.Media.BrushConverter;
using WpfButton = System.Windows.Controls.Button;
using WpfTextBlock = System.Windows.Controls.TextBlock;
using WpfGrid = System.Windows.Controls.Grid;

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
            ShowScenarioDetailsForm();
        }

        private TreeViewItem CreateTreeNode(string text, bool isExpanded = false, bool cyan = false)
        {
            return new TreeViewItem
            {
                Header = CreateTreeHeader(text, null, false, cyan),
                IsExpanded = isExpanded,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch
            };
        }

        private TreeViewItem CreateAddableTreeNode(string text, string childPrefix, bool isExpanded = false, bool cyan = false)
        {
            var node = new TreeViewItem
            {
                IsExpanded = isExpanded,
                Tag = childPrefix,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch
            };

            node.Header = CreateTreeHeader(text, node, true, cyan);
            return node;
        }

        private object CreateTreeHeader(string text, TreeViewItem node, bool showAddButton, bool cyan)
        {
            var headerGrid = new WpfGrid
            {
                Width = 190,
                Height = 22,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            };

            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(170) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) });

            var textBlock = new WpfTextBlock
            {
                Text = text,
                Foreground = cyan
                    ? (WpfBrush)new WpfBrushConverter().ConvertFromString("#21B3AE")
                    : WpfBrushes.White,
                FontWeight = FontWeights.SemiBold,
                FontSize = 13,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                TextAlignment = TextAlignment.Left,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            Grid.SetColumn(textBlock, 0);
            headerGrid.Children.Add(textBlock);

            if (showAddButton && node != null)
            {
                var addButton = new WpfButton
                {
                    Content = "+",
                    Width = 16,
                    Height = 16,
                    Padding = new Thickness(0),
                    Margin = new Thickness(0),
                    Background = WpfBrushes.Transparent,
                    Foreground = WpfBrushes.White,
                    BorderBrush = WpfBrushes.Transparent,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Tag = node
                };

                addButton.Click += AddChildNode_Click;

                Grid.SetColumn(addButton, 1);
                headerGrid.Children.Add(addButton);
            }

            return headerGrid;
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
            ScenarioDescriptionTextBox.Text = scenario.Description ?? string.Empty;
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
                MessageBox.Show(
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

            Btn3D.Background = (WpfBrush)new WpfBrushConverter().ConvertFromString("#062235");
            Btn2D.Background = WpfBrushes.Transparent;
        }

        private void Show2D()
        {
            Host3D.Visibility = Visibility.Collapsed;
            Host2D.Visibility = Visibility.Visible;

            Placeholder3DText.Visibility = Visibility.Collapsed;
            Placeholder2DText.Visibility = Visibility.Collapsed;

            Btn2D.Background = (WpfBrush)new WpfBrushConverter().ConvertFromString("#062235");
            Btn3D.Background = WpfBrushes.Transparent;
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

            var blueLine = CreateTreeNode("BLUE LINE", true);

            var cc = CreateTreeNode("CC", true);
            var cc1 = CreateTreeNode("CC1", true);

            var rdfs = CreateAddableTreeNode("RDFS", "RDFS", true);
            rdfs.Items.Add(CreateTreeNode("RDFS1"));
            
            var jsvushf = CreateAddableTreeNode("JSVUSHF", "JSVUSHF", true);
            jsvushf.Items.Add(CreateTreeNode("JSVUSHF1"));

            cc1.Items.Add(rdfs);
            cc1.Items.Add(jsvushf);
            cc.Items.Add(cc1);

            blueLine.Items.Add(cc);

            var commEmitterAllied = CreateAddableTreeNode("COMMEMITTERALLIED", "COMMEMITTERALLIED", true);
            commEmitterAllied.Items.Add(CreateTreeNode("COMMEMITTERALLIED1"));
            blueLine.Items.Add(commEmitterAllied);

            var radarEmitterAllied = CreateAddableTreeNode("RADAREMITTERALLIED", "RADAREMITTERALLIED", true);
            blueLine.Items.Add(radarEmitterAllied);

            root.Items.Add(blueLine);

            var redLine = CreateTreeNode("RED LINE", true);

            var net = CreateTreeNode("NET", true);
            redLine.Items.Add(net);

            var comEmitter = CreateAddableTreeNode("COMEMITTER", "COMEMITTER", true);
            comEmitter.Items.Add(CreateTreeNode("COMEMITTER1"));
            redLine.Items.Add(comEmitter);

            var radar = CreateAddableTreeNode("RADAR", "RADAR", true);
            radar.Items.Add(CreateTreeNode("RADAR1"));
            redLine.Items.Add(radar);

            root.Items.Add(redLine);

            ScenarioTree.Items.Add(root);
        }

        private void AddChildNode_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            if (sender is not WpfButton button)
                return;

            if (button.Tag is not TreeViewItem parentNode)
                return;

            string prefix = parentNode.Tag?.ToString() ?? "ITEM";

            int nextNumber = GetNextChildNumber(parentNode, prefix);
            string newNodeName = $"{prefix}{nextNumber}";

            parentNode.Items.Add(CreateTreeNode(newNodeName));
            parentNode.IsExpanded = true;
        }

        private int GetNextChildNumber(TreeViewItem parentNode, string prefix)
        {
            int maxNumber = 0;

            foreach (var item in parentNode.Items)
            {
                if (item is not TreeViewItem childNode)
                    continue;

                string text = GetTreeNodeText(childNode);

                if (!text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                string suffix = text.Substring(prefix.Length);

                if (int.TryParse(suffix, out int number) && number > maxNumber)
                {
                    maxNumber = number;
                }
            }

            return maxNumber + 1;
        }

        private string GetTreeNodeText(TreeViewItem item)
        {
            if (item.Header is WpfTextBlock textBlock)
                return textBlock.Text;

            if (item.Header is WpfGrid grid)
            {
                foreach (var child in grid.Children)
                {
                    if (child is WpfTextBlock tb)
                        return tb.Text;
                }
            }

            return item.Header?.ToString() ?? string.Empty;
        }

        private void LoadTablePreview()
        {
            var rows = new List<DrsPreviewRow>
            {
                new() { Sno = 1, Side = "RED",  EmitterName = "COMEMITTER1",        EmitterType = "CUSTOMIZED", Mode = "FF",    Modulation = "AM" },
                new() { Sno = 2, Side = "BLUE", EmitterName = "COMMEMITTERALLIED1", EmitterType = "CUSTOMIZED", Mode = "FH",    Modulation = ""   },
                new() { Sno = 3, Side = "BLUE", EmitterName = "RDFS1",              EmitterType = "CUSTOMIZED", Mode = "SCAN",  Modulation = ""   },
                new() { Sno = 4, Side = "BLUE", EmitterName = "JSVUSHF1",           EmitterType = "CUSTOMIZED", Mode = "FF",    Modulation = "AM" },
                new() { Sno = 5, Side = "RED",  EmitterName = "RADAR1",             EmitterType = "CUSTOMIZED", Mode = "TRACK", Modulation = ""   }
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

        private void ScenarioTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is not TreeViewItem selectedItem)
                return;

            string nodeText = GetTreeNodeText(selectedItem);
            string currentScenarioName = ScenarioNameTextBox.Text?.Trim() ?? string.Empty;

            if (nodeText.Equals(currentScenarioName, StringComparison.OrdinalIgnoreCase))
            {
                ShowScenarioDetailsForm();
            }
            else if (nodeText.Equals("AREA OF OPERATION", StringComparison.OrdinalIgnoreCase))
            {
                ShowAreaOfOperationForm();
            }
            else if (nodeText.Equals("CC1", StringComparison.OrdinalIgnoreCase))
            {
                ShowCcPropertiesForm();
            }
            else if (nodeText.StartsWith("RDFS", StringComparison.OrdinalIgnoreCase)
                     && !nodeText.Equals("RDFS", StringComparison.OrdinalIgnoreCase))
            {
                ShowRdfsPropertiesForm(nodeText);
            }
            else
            {
                ShowEmptyForm();
            }
        }

        private void ShowScenarioDetailsForm()
        {
            ScenarioDetailsBorder.Visibility = Visibility.Visible;
            AreaOfOperationBorder.Visibility = Visibility.Collapsed;
            CcPropertiesBorder.Visibility = Visibility.Collapsed;
            RdfsPropertiesBorder.Visibility = Visibility.Collapsed;
            EmptyDetailsBorder.Visibility = Visibility.Collapsed;
            DrsTableBorder.Visibility = Visibility.Visible;
        }

        private void ShowAreaOfOperationForm()
        {
            ScenarioDetailsBorder.Visibility = Visibility.Collapsed;
            AreaOfOperationBorder.Visibility = Visibility.Visible;
            CcPropertiesBorder.Visibility = Visibility.Collapsed;
            RdfsPropertiesBorder.Visibility = Visibility.Collapsed;
            EmptyDetailsBorder.Visibility = Visibility.Collapsed;
            DrsTableBorder.Visibility = Visibility.Visible;
        }

        private void ShowCcPropertiesForm()
        {
            ScenarioDetailsBorder.Visibility = Visibility.Collapsed;
            AreaOfOperationBorder.Visibility = Visibility.Collapsed;
            CcPropertiesBorder.Visibility = Visibility.Visible;
            RdfsPropertiesBorder.Visibility = Visibility.Collapsed;
            EmptyDetailsBorder.Visibility = Visibility.Collapsed;
            DrsTableBorder.Visibility = Visibility.Visible;
        }

        private void ShowEmptyForm()
        {
            ScenarioDetailsBorder.Visibility = Visibility.Collapsed;
            AreaOfOperationBorder.Visibility = Visibility.Collapsed;
            CcPropertiesBorder.Visibility = Visibility.Collapsed;
            RdfsPropertiesBorder.Visibility = Visibility.Collapsed;
            EmptyDetailsBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;
        }

        private void ShowRdfsPropertiesForm(string rdfsName)
        {
            ScenarioDetailsBorder.Visibility = Visibility.Collapsed;
            AreaOfOperationBorder.Visibility = Visibility.Collapsed;
            CcPropertiesBorder.Visibility = Visibility.Collapsed;
            RdfsPropertiesBorder.Visibility = Visibility.Visible;
            EmptyDetailsBorder.Visibility = Visibility.Collapsed;
            DrsTableBorder.Visibility = Visibility.Visible;

            RdfsTitleTextBlock.Text = $"{rdfsName} PROPERTIES";
            RdfsNameTextBox.Text = rdfsName;

            RdfsLatitudeTextBox.Text = "19° 2' 5.590''";
            RdfsLongitudeTextBox.Text = "77° 21' 15.441''";
            RdfsMinFreqTextBox.Text = "435";
            RdfsMaxFreqTextBox.Text = "1000";
            RdfsSensitivityTextBox.Text = "-90";
            RdfsGainLossTextBox.Text = "3";
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