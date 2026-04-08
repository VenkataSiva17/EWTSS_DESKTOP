using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Infrastructure.Data;
using EWTSS_DESKTOP.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;
using ScenarioModel = EWTSS_DESKTOP.Core.Models.Scenario;
using WpfBrush = System.Windows.Media.Brush;
using WpfBrushConverter = System.Windows.Media.BrushConverter;
using WpfBrushes = System.Windows.Media.Brushes;
using WpfButton = System.Windows.Controls.Button;
using WpfGrid = System.Windows.Controls.Grid;
using WpfTextBlock = System.Windows.Controls.TextBlock;

// Add the correct namespace if your STK controls are in a different namespace


namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioEditorPage : Page
    {
        private readonly int _scenarioId;
        private readonly StkEngineService _stkEngineService;

        private Stk3DViewControl? _stk3DView;
        private Stk2DViewControl? _stk2DView;


        private int? _selectedCcId;
        private int? _selectedEntityId;
        private int? _selectedEmitterId;

        private int? _selectedAreaOperationId;

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
        private void SetAreaOperationButtonState(bool isUpdate)
        {
            if (isUpdate)
            {
                CreateAreaOperationButton.Content = "UPDATE";
            }
            else
            {
                CreateAreaOperationButton.Content = "CREATE";
            }
        }
        private void SetRdfsButtonState(bool isUpdate)
        {
            if (isUpdate)
                CreateRdfsButton.Content = "UPDATE";
            else
                CreateRdfsButton.Content = "CREATE";
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

        private object CreateTreeHeader(string text, TreeViewItem? node, bool showAddButton, bool cyan)
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
                    ? (WpfBrush)new WpfBrushConverter().ConvertFromString("#21B3AE")!
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
            try
            {
                using var db = new AppDbContext();

                var scenario = db.Scenarios
                    .Where(x => x.Id == _scenarioId)
                    .Select(x => new
                    {
                        Scenario = x,
                        UserFirstName = x.User != null ? x.User.FirstName : null,
                        UserLastName = x.User != null ? x.User.LastName : null
                    })
                    .FirstOrDefault();

                if (scenario == null)
                {
                    MessageBox.Show("Scenario not found.");
                    return;
                }

                var scenarioData = scenario.Scenario;

                ScenarioTitleTextBlock.Text = $"{scenarioData.Name} | - TREE STRUCTURE";
                UserNameTextBlock.Text = !string.IsNullOrWhiteSpace(scenario.UserFirstName)
                    ? $"{scenario.UserFirstName} {scenario.UserLastName}"
                    : "Admin Admin";

                ScenarioNameTextBox.Text = scenarioData.Name ?? string.Empty;
                ScenarioDescriptionTextBox.Text = scenarioData.Description ?? string.Empty;
                ScenarioCreatedDateTextBlock.Text = $"SCENARIO CREATION DATE (DD-MM-YYYY) : {scenarioData.StartDate:dd-MM-yyyy}";
                ScenarioCreatedTimeTextBlock.Text = $"SCENARIO CREATION TIME (HH:MM:SS) : {scenarioData.StartTime}";
                ScenarioDurationTextBox.Text = scenarioData.Duration.ToString();
                ScenarioExecutionTimeTextBox.Text = scenarioData.ExecuteTime?.ToString() ?? "--:--:--";

                UpdateDescriptionCount();
                LoadScenarioTree(scenarioData);
                InitializeStkViews(scenarioData.Name ?? "Scenario");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load scenario.\n\n{ex.Message}");
            }
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

            Btn3D.Background = (WpfBrush)new WpfBrushConverter().ConvertFromString("#062235")!;
            Btn2D.Background = WpfBrushes.Transparent;
        }

        private void Show2D()
        {
            Host3D.Visibility = Visibility.Collapsed;
            Host2D.Visibility = Visibility.Visible;

            Placeholder3DText.Visibility = Visibility.Collapsed;
            Placeholder2DText.Visibility = Visibility.Collapsed;

            Btn2D.Background = (WpfBrush)new WpfBrushConverter().ConvertFromString("#062235")!;
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

            var root = CreateTreeNode(scenario.Name ?? "Scenario", true, true);

            root.Items.Add(CreateTreeNode("AREA OF OPERATION"));

            var blueLine = CreateTreeNode("BLUE LINE", false);

            var cc = CreateTreeNode("CC", false);
            var cc1 = CreateTreeNode("CC1", false);

            var rdfs = CreateAddableTreeNode("RDFS", "RDFS", false);
            rdfs.Items.Add(CreateTreeNode("RDFS1"));

            var jsvushf = CreateAddableTreeNode("JSVUSHF", "JSVUSHF", false);
            jsvushf.Items.Add(CreateTreeNode("JSVUSHF1"));

            cc1.Items.Add(rdfs);
            cc1.Items.Add(jsvushf);
            cc.Items.Add(cc1);
            blueLine.Items.Add(cc);

            var commEmitterAllied = CreateAddableTreeNode("COMMEMITTERALLIED", "COMMEMITTERALLIED", false);
            commEmitterAllied.Items.Add(CreateTreeNode("COMMEMITTERALLIED1"));
            blueLine.Items.Add(commEmitterAllied);

            var radarEmitterAllied = CreateAddableTreeNode("RADAREMITTERALLIED", "RADAREMITTERALLIED", false);
            blueLine.Items.Add(radarEmitterAllied);

            root.Items.Add(blueLine);

            var redLine = CreateTreeNode("RED LINE", false);

            var net = CreateTreeNode("NET", false);
            redLine.Items.Add(net);

            var comEmitter = CreateAddableTreeNode("COMEMITTER", "COMEMITTER", false);
            comEmitter.Items.Add(CreateTreeNode("COMEMITTER1"));
            redLine.Items.Add(comEmitter);

            var radar = CreateAddableTreeNode("RADAR", "RADAR", false);
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
            try
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
                scenario.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                ScenarioTitleTextBlock.Text = $"{scenario.Name} | - TREE STRUCTURE";
                MessageBox.Show("Scenario updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update scenario.\n\n{ex.Message}");
            }
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
                LoadAreaOperationFromDb();
            }
            else if (nodeText.Equals("BLUE LINE", StringComparison.OrdinalIgnoreCase) ||
                     nodeText.Equals("RED LINE", StringComparison.OrdinalIgnoreCase))
            {
                ShowEmptyForm();
            }
            else if (nodeText.Equals("CC", StringComparison.OrdinalIgnoreCase))
            {
                _selectedCcId = null;
                CcNameTextBox.Text = "CC1";
                CcLatitudeTextBox.Text = "";
                CcLongitudeTextBox.Text = "";
                ShowCcPropertiesForm();
            }
            else if (nodeText.StartsWith("CC", StringComparison.OrdinalIgnoreCase) &&
                     !nodeText.Equals("CC", StringComparison.OrdinalIgnoreCase))
            {
                LoadCcFromDbByName(nodeText);
            }
            else if (nodeText.Equals("RDFS", StringComparison.OrdinalIgnoreCase))
            {
                ShowEmptyForm();
            }
            else if (nodeText.StartsWith("RDFS", StringComparison.OrdinalIgnoreCase) &&
                     !nodeText.Equals("RDFS", StringComparison.OrdinalIgnoreCase))
            {
                LoadRdfsFromDbByName(nodeText);
            }
            else if (nodeText.Equals("JSVUSHF", StringComparison.OrdinalIgnoreCase))
            {
                ShowEmptyForm();
            }
            else if (nodeText.StartsWith("JSVUSHF", StringComparison.OrdinalIgnoreCase) &&
                     !nodeText.Equals("JSVUSHF", StringComparison.OrdinalIgnoreCase))
            {
                LoadJsvushfFromDbByName(nodeText);
            }
            else if (nodeText.Equals("COMMEMITTERALLIED", StringComparison.OrdinalIgnoreCase))
            {
                ShowEmptyForm();
            }
            else if (nodeText.StartsWith("COMMEMITTERALLIED", StringComparison.OrdinalIgnoreCase) &&
                     !nodeText.Equals("COMMEMITTERALLIED", StringComparison.OrdinalIgnoreCase))
            {
                LoadCommEmitterAlliedFromDbByName(nodeText);
            }
            else
            {
                ShowEmptyForm();
            }
        }
        private void ShowCommEmitterTechnicalTab()
        {
            CommEmitterTechnicalScrollViewer.Visibility = Visibility.Visible;
            CommEmitterTacticalScrollViewer.Visibility = Visibility.Collapsed;

            CommEmitterTechnicalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#2CB7B0");
            CommEmitterTacticalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#179C96");
        }

        private void ShowCommEmitterTacticalTab()
        {
            CommEmitterTechnicalScrollViewer.Visibility = Visibility.Collapsed;
            CommEmitterTacticalScrollViewer.Visibility = Visibility.Visible;

            CommEmitterTechnicalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#179C96");
            CommEmitterTacticalTabButton.Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#2CB7B0");
        }

        private void CommEmitterTechnicalTabButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCommEmitterTechnicalTab();
        }

        private void CommEmitterTacticalTabButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCommEmitterTacticalTab();
        }
        private void CreateScenario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                TimeSpan.TryParse(ScenarioExecutionTimeTextBox.Text, out var executeTime);

                var scenario = new ScenarioModel
                {
                    Name = ScenarioNameTextBox.Text?.Trim(),
                    Description = ScenarioDescriptionTextBox.Text?.Trim(),
                    StartDate = DateTime.Now.Date,
                    StartTime = DateTime.Now.TimeOfDay,
                    ExecuteDate = DateTime.Now.Date,
                    ExecuteTime = executeTime,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = true,
                    UserId = 1
                };

                db.Scenarios.Add(scenario);
                db.SaveChanges();

                MessageBox.Show("Scenario created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create scenario.\n\n{ex.Message}");
            }
        }

        private void CreateAreaOperation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int scenarioId = _scenarioId > 0
                    ? _scenarioId
                    : db.Scenarios
                        .OrderByDescending(x => x.Id)
                        .Select(x => x.Id)
                        .FirstOrDefault();

                if (scenarioId <= 0)
                {
                    MessageBox.Show("Please create a scenario first.");
                    return;
                }

                AreaOperation areaOperation;
                bool isUpdate = false;

                if (_selectedAreaOperationId.HasValue)
                {
                    areaOperation = db.AreaOperations.FirstOrDefault(x => x.Id == _selectedAreaOperationId.Value);
                    if (areaOperation == null)
                    {
                        MessageBox.Show("Selected Area Of Operation not found.");
                        return;
                    }

                    isUpdate = true;
                }
                else
                {
                    areaOperation = db.AreaOperations.FirstOrDefault(x => x.ScenarioId == scenarioId);

                    if (areaOperation == null)
                    {
                        areaOperation = new AreaOperation
                        {
                            CreatedOn = DateTime.Now,
                            IsActive = true,
                            ScenarioId = scenarioId
                        };

                        db.AreaOperations.Add(areaOperation);
                    }
                    else
                    {
                        isUpdate = true;
                    }
                }

                areaOperation.Name = "AREA OF OPERATION";
                areaOperation.Description = "Created from Scenario Editor";
                areaOperation.Altitude = "1000";
                areaOperation.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                bool hasBlueLine = db.ScenarioLines.Any(x =>
                    x.AreaOperationId == areaOperation.Id && x.Name == "BLUE LINE");

                bool hasRedLine = db.ScenarioLines.Any(x =>
                    x.AreaOperationId == areaOperation.Id && x.Name == "RED LINE");

                if (!hasBlueLine)
                {
                    db.ScenarioLines.Add(new ScenarioLine
                    {
                        Name = "BLUE LINE",
                        Description = "Auto-created for Area Of Operation",
                        AreaOperationId = areaOperation.Id,
                        LineType = LineType.BULE_LINE,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = true
                    });
                }

                if (!hasRedLine)
                {
                    db.ScenarioLines.Add(new ScenarioLine
                    {
                        Name = "RED LINE",
                        Description = "Auto-created for Area Of Operation",
                        AreaOperationId = areaOperation.Id,
                        LineType = LineType.RED_LINE,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = true
                    });
                }

                db.SaveChanges();

                _selectedAreaOperationId = areaOperation.Id;

                SetAreaOperationButtonState(true);

                MessageBox.Show(isUpdate
                    ? "Area Of Operation updated successfully."
                    : "Area Of Operation created successfully.");

                LoadScenario();
            }
            catch (Exception ex)
            {
                string fullError = ex.Message;
                Exception? inner = ex.InnerException;

                while (inner != null)
                {
                    fullError += "\n\nINNER ERROR:\n" + inner.Message;
                    inner = inner.InnerException;
                }

                MessageBox.Show(fullError, "Failed to save Area Of Operation");
            }
        }
        private void CreateOrUpdateAreaOperation_Click(object sender, RoutedEventArgs e)
        {
            CreateAreaOperation_Click(sender, e);
        }

        private void CreateCc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int blueLineId = db.ScenarioLines
                    .Where(x => x.AreaOperation.ScenarioId == _scenarioId
                             && x.Name == "BLUE LINE")
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (blueLineId <= 0)
                {
                    MessageBox.Show("BLUE LINE not found for this scenario.");
                    return;
                }

                var cc = new Cc
                {
                    CcName = string.IsNullOrWhiteSpace(CcNameTextBox.Text)
                        ? "CC1"
                        : CcNameTextBox.Text.Trim(),

                    Latitude = string.IsNullOrWhiteSpace(CcLatitudeTextBox.Text)
                        ? "0"
                        : CcLatitudeTextBox.Text.Trim(),

                    Longitude = string.IsNullOrWhiteSpace(CcLongitudeTextBox.Text)
                        ? "0"
                        : CcLongitudeTextBox.Text.Trim(),

                    LineId = blueLineId,


                };

                db.Ccs.Add(cc);
                db.SaveChanges();

                MessageBox.Show("CC created successfully.");
            }
            catch (Exception ex)
            {
                string fullError = ex.Message;

                Exception inner = ex.InnerException;
                while (inner != null)
                {
                    fullError += "\n\nINNER ERROR:\n" + inner.Message;
                    inner = inner.InnerException;
                }

                MessageBox.Show(fullError, "Failed to create CC");
            }

        }
        private void CreateRdfs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int ccId = _selectedCcId ?? db.Ccs
                    .Where(x => x.Line.AreaOperation.ScenarioId == _scenarioId)
                    .OrderByDescending(x => x.Id)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (ccId <= 0)
                {
                    MessageBox.Show("Please create CC first before creating RDFS.");
                    return;
                }

                if (!TryParseDouble(RdfsMinFreqTextBox.Text, out double minFreq) ||
                    !TryParseDouble(RdfsMaxFreqTextBox.Text, out double maxFreq))
                {
                    MessageBox.Show("Please enter valid RDFS frequency values.");
                    return;
                }

                int? antennaHeight = null;
                if (int.TryParse(RdfsAntennaHeightTextBox.Text, out int parsedHeight))
                    antennaHeight = parsedHeight;

                Entity entity;
                if (_selectedEntityId.HasValue)
                {
                    entity = db.Entities.FirstOrDefault(x => x.Id == _selectedEntityId.Value);
                    if (entity == null)
                    {
                        MessageBox.Show("Selected RDFS not found.");
                        return;
                    }
                }
                else
                {
                    entity = new Entity();
                    db.Entities.Add(entity);
                }

                entity.Name = string.IsNullOrWhiteSpace(RdfsNameTextBox.Text) ? "RDFS1" : RdfsNameTextBox.Text.Trim();
                entity.StartFrequencyValue = minFreq;
                entity.StopFrequencyValue = maxFreq;
                entity.AntennaType = string.IsNullOrWhiteSpace(RdfsAntennaTypeTextBox.Text) ? "Default" : RdfsAntennaTypeTextBox.Text.Trim();
                entity.Polarization = string.IsNullOrWhiteSpace(RdfsPolarizationTextBox.Text) ? "Vertical" : RdfsPolarizationTextBox.Text.Trim();
                entity.AntennaHeight = antennaHeight;
                entity.ScanType = string.IsNullOrWhiteSpace(RdfsScanTypeTextBox.Text) ? "SCAN" : RdfsScanTypeTextBox.Text.Trim();
                entity.CcId = ccId;
                entity.EntityType = EntityType.RDFS;

                db.SaveChanges();

                MessageBox.Show(_selectedEntityId.HasValue ? "RDFS updated successfully." : "RDFS saved successfully.");
                LoadScenario();
            }
            catch (Exception ex)
            {
                string fullError = ex.Message;
                Exception? inner = ex.InnerException;

                while (inner != null)
                {
                    fullError += "\n\nINNER ERROR:\n" + inner.Message;
                    inner = inner.InnerException;
                }

                MessageBox.Show(fullError, "Failed to save RDFS");
            }
        }
        private void CreateJsvushf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int ccId = _selectedCcId ?? db.Ccs
                    .Where(x => x.Line.AreaOperation.ScenarioId == _scenarioId)
                    .OrderByDescending(x => x.Id)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (ccId <= 0)
                {
                    MessageBox.Show("Please create CC first before creating JSVUSHF.");
                    return;
                }

                if (!TryParseDouble(JsvushfMinFreqTextBox.Text, out double minFreq) ||
                    !TryParseDouble(JsvushfMaxFreqTextBox.Text, out double maxFreq))
                {
                    MessageBox.Show("Please enter valid frequency values.");
                    return;
                }

                int? antennaHeight = null;
                if (int.TryParse(JsvushfAntennaHeightTextBox.Text, out int parsedHeight))
                    antennaHeight = parsedHeight;

                Entity entity;
                if (_selectedEntityId.HasValue)
                {
                    entity = db.Entities.FirstOrDefault(x => x.Id == _selectedEntityId.Value);
                    if (entity == null)
                    {
                        MessageBox.Show("Selected JSVUSHF not found.");
                        return;
                    }
                }
                else
                {
                    entity = new Entity();
                    db.Entities.Add(entity);
                }

                entity.Name = string.IsNullOrWhiteSpace(JsvushfNameTextBox.Text) ? "JSVUSHF1" : JsvushfNameTextBox.Text.Trim();
                entity.StartFrequencyValue = minFreq;
                entity.StopFrequencyValue = maxFreq;
                entity.AntennaType = string.IsNullOrWhiteSpace(JsvushfAntennaTypeTextBox.Text) ? "Default" : JsvushfAntennaTypeTextBox.Text.Trim();
                entity.Polarization = string.IsNullOrWhiteSpace(JsvushfPolarizationTextBox.Text) ? "Vertical" : JsvushfPolarizationTextBox.Text.Trim();
                entity.AntennaHeight = antennaHeight;
                entity.ScanType = string.IsNullOrWhiteSpace(JsvushfScanTypeTextBox.Text) ? "SCAN" : JsvushfScanTypeTextBox.Text.Trim();
                entity.CcId = ccId;
                entity.EntityType = EntityType.JSVUSHF;

                db.SaveChanges();

                MessageBox.Show(_selectedEntityId.HasValue ? "JSVUSHF updated successfully." : "JSVUSHF saved successfully.");
                LoadScenario();
            }
            catch (Exception ex)
            {
                string fullError = ex.Message;
                Exception? inner = ex.InnerException;

                while (inner != null)
                {
                    fullError += "\n\nINNER ERROR:\n" + inner.Message;
                    inner = inner.InnerException;
                }

                MessageBox.Show(fullError, "Failed to save JSVUSHF");
            }
        }
        private void CreateCommEmitterAllied_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new AppDbContext();

                int blueLineId = db.ScenarioLines
                    .Where(x => x.AreaOperation.ScenarioId == _scenarioId && x.Name == "BLUE LINE")
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (blueLineId <= 0)
                {
                    MessageBox.Show("BLUE LINE not found for this scenario.");
                    return;
                }

                double? power = null;
                if (double.TryParse(CommEmitterPowerTextBox.Text, out double parsedPower))
                    power = parsedPower;

                double? frequency = null;
                if (double.TryParse(CommEmitterFrequencyTextBox.Text, out double parsedFrequency))
                    frequency = parsedFrequency;

                double? bandwidth = null;
                if (double.TryParse(CommEmitterBandwidthTextBox.Text, out double parsedBandwidth))
                    bandwidth = parsedBandwidth;

                double? gain = null;
                if (double.TryParse(CommEmitterGainTextBox.Text, out double parsedGain))
                    gain = parsedGain;

                double? scanRate = null;
                if (double.TryParse(CommEmitterScanRateTextBox.Text, out double parsedScanRate))
                    scanRate = parsedScanRate;

                Emitter emitter;
                if (_selectedEmitterId.HasValue)
                {
                    emitter = db.Emitters.FirstOrDefault(x => x.Id == _selectedEmitterId.Value);
                    if (emitter == null)
                    {
                        MessageBox.Show("Selected COMMEMITTERALLIED not found.");
                        return;
                    }
                }
                else
                {
                    emitter = new Emitter
                    {
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };
                    db.Emitters.Add(emitter);
                }

                emitter.Name = string.IsNullOrWhiteSpace(CommEmitterNameTextBox.Text)
                    ? "COMMEMITTERALLIED1"
                    : CommEmitterNameTextBox.Text.Trim();

                emitter.PlatformType = PlatformType.LAND_STATIC;
                emitter.ModeType = (CommEmitterModeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "FF";
                emitter.PowerTransmitted = power;
                emitter.StartFrequencyValue = frequency;
                emitter.StopFrequencyValue = frequency;
                emitter.Bandwidth = bandwidth;
                emitter.HopPeriodValue = 0;
                emitter.HopPeriodUnit = "ms";
                emitter.HopInterPeriodValue = 0;
                emitter.HopInterPeriodUnit = "ms";
                emitter.ModulationType = "AM";
                emitter.PatternType = "DEFAULT";
                emitter.ScanRate = scanRate;
                emitter.AntennaType = string.IsNullOrWhiteSpace(CommEmitterAntennaTypeTextBox.Text)
                    ? "DEFAULT"
                    : CommEmitterAntennaTypeTextBox.Text.Trim();
                emitter.Gain = gain;
                emitter.Polarization = string.IsNullOrWhiteSpace(CommEmitterPolarizationTextBox.Text)
                    ? "VERTICAL"
                    : CommEmitterPolarizationTextBox.Text.Trim();
                emitter.LineId = blueLineId;
                emitter.EmitterType = (CommEmitterEmitterTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "COMMUNICATION";
                emitter.Type = EmitterKeyName.COMMEMITTERALLIED;
                emitter.UpdatedOn = DateTime.Now;

                db.SaveChanges();

                MessageBox.Show(_selectedEmitterId.HasValue
                    ? "COMMEMITTERALLIED updated successfully."
                    : "COMMEMITTERALLIED saved successfully.");

                LoadScenario();
            }
            catch (Exception ex)
            {
                string fullError = ex.Message;
                Exception? inner = ex.InnerException;

                while (inner != null)
                {
                    fullError += "\n\nINNER ERROR:\n" + inner.Message;
                    inner = inner.InnerException;
                }

                MessageBox.Show(fullError, "Failed to save COMMEMITTERALLIED");
            }
        }

        private static bool TryParseDouble(string? value, out double result)
        {
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                   || double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result);
        }

        private void HideAllForms()
        {
            ScenarioDetailsBorder.Visibility = Visibility.Collapsed;
            AreaOfOperationBorder.Visibility = Visibility.Collapsed;
            CcPropertiesBorder.Visibility = Visibility.Collapsed;
            RdfsPropertiesBorder.Visibility = Visibility.Collapsed;
            JsvushfPropertiesBorder.Visibility = Visibility.Collapsed;
            CommEmitterAlliedPropertiesBorder.Visibility = Visibility.Collapsed;
            EmptyDetailsBorder.Visibility = Visibility.Collapsed;
        }

        private void ShowScenarioDetailsForm()
        {
            HideAllForms();
            ScenarioDetailsBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;
        }

        private void ShowAreaOfOperationForm()
        {
            HideAllForms();
            AreaOfOperationBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;
        }

        private void ShowCcPropertiesForm()
        {
            HideAllForms();
            CcPropertiesBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;
        }

        private void ShowEmptyForm()
        {
            HideAllForms();
            EmptyDetailsBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;
        }

        private void ShowRdfsPropertiesForm(string rdfsName)
        {
            HideAllForms();
            RdfsPropertiesBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;

            RdfsTitleTextBlock.Text = $"{rdfsName} PROPERTIES";
            RdfsNameTextBox.Text = rdfsName;

            RdfsLatitudeTextBox.Text = "19° 2' 5.590''";
            RdfsLongitudeTextBox.Text = "77° 21' 15.441''";
            RdfsMinFreqTextBox.Text = "435";
            RdfsMaxFreqTextBox.Text = "1000";
            RdfsSensitivityTextBox.Text = "-90";
            RdfsGainLossTextBox.Text = "3";

            RdfsAntennaTypeTextBox.Text = "Default";
            RdfsPolarizationTextBox.Text = "Vertical";
            RdfsAntennaHeightTextBox.Text = "10";
            RdfsScanTypeTextBox.Text = "SCAN";
        }

        private void ShowJsvushfPropertiesForm(string name)
        {
            HideAllForms();
            JsvushfPropertiesBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;

            JsvushfTitleTextBlock.Text = $"{name} PROPERTIES";
            JsvushfNameTextBox.Text = name;

            JsvushfLatitudeTextBox.Text = "21° 24' 54.318''";
            JsvushfLongitudeTextBox.Text = "77° 36' 49.554''";
            JsvushfMinFreqTextBox.Text = "435";
            JsvushfMaxFreqTextBox.Text = "1000";
            JsvushfSensitivityTextBox.Text = "-90";
            JsvushfGainLossTextBox.Text = "0";

            JsvushfAntennaTypeTextBox.Text = "Default";
            JsvushfPolarizationTextBox.Text = "Vertical";
            JsvushfAntennaHeightTextBox.Text = "10";
            JsvushfScanTypeTextBox.Text = "SCAN";
        }

        private void ShowCommEmitterAlliedPropertiesForm(string name)
        {
            HideAllForms();
            CommEmitterAlliedPropertiesBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;

            CommEmitterAlliedTitleTextBlock.Text = $"{name} PROPERTIES";
            CommEmitterNameTextBox.Text = name;

            CommEmitterPlatformTypeComboBox.SelectedIndex = 0;
            CommEmitterModeComboBox.SelectedIndex = 0;
            CommEmitterGainTextBox.Text = "0";
            CommEmitterPowerTextBox.Text = "60";
            CommEmitterBandwidthTextBox.Text = "3";
            CommEmitterFrequencyTextBox.Text = "1000";
            CommEmitterAntennaTypeTextBox.Text = "DEFAULT";
            CommEmitterPolarizationTextBox.Text = "VERTICAL";
            CommEmitterScanRateTextBox.Text = "0";
            CommEmitterDurationTextBox.Text = "00:10:00";
            CommEmitterSequenceCountTextBox.Text = "1";

            CommEmitterSequenceDataGrid.ItemsSource = new List<EmitterSequenceRow>
    {
        new EmitterSequenceRow
        {
            Sno = 1,
            Start = "00:00:00",
            Stop = "00:10:00",
            Duration = 600,
            Mode = "ACTIVE"
        }
    };

            ShowCommEmitterTechnicalTab();
        }
        private void LoadCcFromDbByName(string ccName)
        {
            using var db = new AppDbContext();

            var cc = db.Ccs
                .Where(x => x.Line.AreaOperation.ScenarioId == _scenarioId)
                .AsEnumerable()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.CcName?.Trim(),
                        ccName.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (cc == null)
            {
                _selectedCcId = null;
                CcNameTextBox.Text = ccName;
                CcLatitudeTextBox.Text = "";
                CcLongitudeTextBox.Text = "";
                ShowCcPropertiesForm();
                return;
            }

            _selectedCcId = cc.Id;
            CcNameTextBox.Text = cc.CcName ?? "";
            CcLatitudeTextBox.Text = cc.Latitude ?? "";
            CcLongitudeTextBox.Text = cc.Longitude ?? "";

            ShowCcPropertiesForm();
        }
        private void LoadRdfsFromDbByName(string entityName)
        {
            using var db = new AppDbContext();

            var entity = db.Entities
                .Where(x => x.Cc.Line.AreaOperation.ScenarioId == _scenarioId
                         && x.EntityType == EntityType.RDFS)
                .AsEnumerable()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Name?.Trim(),
                        entityName.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (entity == null)
            {
                _selectedEntityId = null;

                // keep your old default form structure and default values
                ShowRdfsPropertiesForm(entityName);
                RdfsNameTextBox.Text = entityName;

                SetRdfsButtonState(false);   // CREATE
                return;
            }

            _selectedEntityId = entity.Id;
            _selectedCcId = entity.CcId;

            // first load normal form structure
            ShowRdfsPropertiesForm(entity.Name ?? "RDFS");

            // then replace defaults with DB values
            RdfsNameTextBox.Text = entity.Name ?? "RDFS1";
            RdfsMinFreqTextBox.Text = entity.StartFrequencyValue?.ToString() ?? "435";
            RdfsMaxFreqTextBox.Text = entity.StopFrequencyValue?.ToString() ?? "1000";
            RdfsAntennaTypeTextBox.Text = entity.AntennaType ?? "Default";
            RdfsPolarizationTextBox.Text = entity.Polarization ?? "Vertical";
            RdfsAntennaHeightTextBox.Text = entity.AntennaHeight?.ToString() ?? "10";
            RdfsScanTypeTextBox.Text = entity.ScanType ?? "SCAN";

            SetRdfsButtonState(true);   // UPDATE
        }

        private void LoadJsvushfFromDbByName(string entityName)
        {
            using var db = new AppDbContext();

            var entity = db.Entities
                .Where(x => x.Cc.Line.AreaOperation.ScenarioId == _scenarioId
                         && x.EntityType == EntityType.JSVUSHF)
                .AsEnumerable()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Name?.Trim(),
                        entityName.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (entity == null)
            {
                _selectedEntityId = null;

                ShowJsvushfPropertiesForm(entityName);

                JsvushfNameTextBox.Text = entityName;
                JsvushfMinFreqTextBox.Text = "";
                JsvushfMaxFreqTextBox.Text = "";
                JsvushfAntennaTypeTextBox.Text = "";
                JsvushfPolarizationTextBox.Text = "";
                JsvushfAntennaHeightTextBox.Text = "";
                JsvushfScanTypeTextBox.Text = "";

                return;
            }

            _selectedEntityId = entity.Id;
            _selectedCcId = entity.CcId;

            ShowJsvushfPropertiesForm(entity.Name ?? "JSVUSHF");

            JsvushfNameTextBox.Text = entity.Name ?? string.Empty;
            JsvushfMinFreqTextBox.Text = entity.StartFrequencyValue?.ToString() ?? string.Empty;
            JsvushfMaxFreqTextBox.Text = entity.StopFrequencyValue?.ToString() ?? string.Empty;
            JsvushfAntennaTypeTextBox.Text = entity.AntennaType ?? string.Empty;
            JsvushfPolarizationTextBox.Text = entity.Polarization ?? string.Empty;
            JsvushfAntennaHeightTextBox.Text = entity.AntennaHeight?.ToString() ?? string.Empty;
            JsvushfScanTypeTextBox.Text = entity.ScanType ?? string.Empty;
        }

        private void LoadCommEmitterAlliedFromDbByName(string emitterName)
        {
            using var db = new AppDbContext();

            var emitter = db.Emitters
                .Where(x => x.Line.AreaOperation.ScenarioId == _scenarioId
                         && x.Type == EmitterKeyName.COMMEMITTERALLIED)
                .AsEnumerable()
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Name?.Trim(),
                        emitterName.Trim(),
                        StringComparison.OrdinalIgnoreCase));

            if (emitter == null)
            {
                _selectedEmitterId = null;

                ShowCommEmitterAlliedPropertiesForm(emitterName);

                CommEmitterNameTextBox.Text = emitterName;
                CommEmitterPowerTextBox.Text = "";
                CommEmitterBandwidthTextBox.Text = "";
                CommEmitterFrequencyTextBox.Text = "";
                CommEmitterAntennaTypeTextBox.Text = "";
                CommEmitterPolarizationTextBox.Text = "";
                CommEmitterScanRateTextBox.Text = "";
                CommEmitterGainTextBox.Text = "";

                return;
            }

            _selectedEmitterId = emitter.Id;

            ShowCommEmitterAlliedPropertiesForm(emitter.Name ?? "COMMEMITTERALLIED");

            CommEmitterNameTextBox.Text = emitter.Name ?? string.Empty;
            CommEmitterPowerTextBox.Text = emitter.PowerTransmitted?.ToString() ?? string.Empty;
            CommEmitterBandwidthTextBox.Text = emitter.Bandwidth?.ToString() ?? string.Empty;
            CommEmitterFrequencyTextBox.Text = emitter.StartFrequencyValue?.ToString() ?? string.Empty;
            CommEmitterAntennaTypeTextBox.Text = emitter.AntennaType ?? string.Empty;
            CommEmitterPolarizationTextBox.Text = emitter.Polarization ?? string.Empty;
            CommEmitterScanRateTextBox.Text = emitter.ScanRate?.ToString() ?? string.Empty;
            CommEmitterGainTextBox.Text = emitter.Gain?.ToString() ?? string.Empty;
        }
        private void LoadAreaOperationFromDb()
        {
            using var db = new AppDbContext();

            var area = db.AreaOperations
                .Where(x => x.ScenarioId == _scenarioId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            ShowAreaOfOperationForm();

            if (area == null)
            {
                _selectedAreaOperationId = null;

                SetAreaOperationButtonState(false); // CREATE

                return;
            }

            _selectedAreaOperationId = area.Id;

            // load fields if needed
            // AreaOperationNameTextBox.Text = area.Name;

            SetAreaOperationButtonState(true); // UPDATE
        }


        public class DrsPreviewRow
        {
            public int Sno { get; set; }
            public string Side { get; set; } = string.Empty;
            public string EmitterName { get; set; } = string.Empty;
            public string EmitterType { get; set; } = string.Empty;
            public string Mode { get; set; } = string.Empty;
            public string Modulation { get; set; } = string.Empty;
        }
        public class EmitterSequenceRow
        {
            public int Sno { get; set; }
            public string Start { get; set; } = string.Empty;
            public string Stop { get; set; } = string.Empty;
            public int Duration { get; set; }
            public string Mode { get; set; } = string.Empty;
        }


    }
}
