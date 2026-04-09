using EWTSS_DESKTOP.Core.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfBrush = System.Windows.Media.Brush;
using WpfBrushConverter = System.Windows.Media.BrushConverter;
using WpfBrushes = System.Windows.Media.Brushes;
using WpfButton = System.Windows.Controls.Button;
using WpfGrid = System.Windows.Controls.Grid;
using WpfTextBlock = System.Windows.Controls.TextBlock;
using ScenarioModel = EWTSS_DESKTOP.Core.Models.Scenario;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioEditorPage
    {
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

        private void LoadScenarioTree(ScenarioModel scenario)
        {
            ScenarioTree.Items.Clear();

            var root = CreateTreeNode(scenario.Name ?? "Scenario", true, true);

            root.Items.Add(CreateTreeNode("AREA OF OPERATION"));

            var blueLine = CreateTreeNode("BLUE LINE");
            var cc = CreateTreeNode("CC");
            var cc1 = CreateTreeNode("CC1");

            var rdfs = CreateAddableTreeNode("RDFS", "RDFS");
            rdfs.Items.Add(CreateTreeNode("RDFS1"));

            var jsvushf = CreateAddableTreeNode("JSVUSHF", "JSVUSHF");
            jsvushf.Items.Add(CreateTreeNode("JSVUSHF1"));

            cc1.Items.Add(rdfs);
            cc1.Items.Add(jsvushf);
            cc.Items.Add(cc1);
            blueLine.Items.Add(cc);

            var commEmitterAllied = CreateAddableTreeNode("COMMEMITTERALLIED", "COMMEMITTERALLIED");
            commEmitterAllied.Items.Add(CreateTreeNode("COMMEMITTERALLIED1"));
            blueLine.Items.Add(commEmitterAllied);

            var radarEmitterAllied = CreateAddableTreeNode("RADAREMITTERALLIED", "RADAREMITTERALLIED");
            radarEmitterAllied.Items.Add(CreateTreeNode("RADAREMITTERALLIED1"));
            blueLine.Items.Add(radarEmitterAllied);

            root.Items.Add(blueLine);

            var redLine = CreateTreeNode("RED LINE");
            var net = CreateTreeNode("NET");
            redLine.Items.Add(net);

            var comEmitter = CreateAddableTreeNode("COMEMITTER", "COMEMITTER");
            comEmitter.Items.Add(CreateTreeNode("COMEMITTER1"));
            redLine.Items.Add(comEmitter);

            var radar = CreateAddableTreeNode("RADAR", "RADAR");
            radar.Items.Add(CreateTreeNode("RADAR1"));
            redLine.Items.Add(radar);

            root.Items.Add(redLine);

            ScenarioTree.Items.Add(root);
        }

        private void AddChildNode_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            if (sender is not WpfButton button || button.Tag is not TreeViewItem parentNode)
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
                    maxNumber = number;
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

        private void ScenarioTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is not TreeViewItem selectedItem)
                return;

            string nodeText = GetTreeNodeText(selectedItem);
            string currentScenarioName = ScenarioNameTextBox.Text?.Trim() ?? string.Empty;

            if (nodeText.Equals(currentScenarioName, StringComparison.OrdinalIgnoreCase))
                ShowScenarioDetailsForm();
            else if (nodeText.Equals("AREA OF OPERATION", StringComparison.OrdinalIgnoreCase))
                LoadAreaOperationFromDb();
            else if (nodeText.Equals("BLUE LINE", StringComparison.OrdinalIgnoreCase) ||
                     nodeText.Equals("RED LINE", StringComparison.OrdinalIgnoreCase))
                ShowEmptyForm();
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
                LoadCcFromDbByName(nodeText);
            else if (nodeText.StartsWith("RDFS", StringComparison.OrdinalIgnoreCase) &&
                     !nodeText.Equals("RDFS", StringComparison.OrdinalIgnoreCase))
                LoadRdfsFromDbByName(nodeText);
            else if (nodeText.StartsWith("JSVUSHF", StringComparison.OrdinalIgnoreCase) &&
                     !nodeText.Equals("JSVUSHF", StringComparison.OrdinalIgnoreCase))
                LoadJsvushfFromDbByName(nodeText);
            else if (nodeText.StartsWith("COMMEMITTERALLIED", StringComparison.OrdinalIgnoreCase) &&
                     !nodeText.Equals("COMMEMITTERALLIED", StringComparison.OrdinalIgnoreCase))
                LoadCommEmitterAlliedFromDbByName(nodeText);
            else if (nodeText.StartsWith("RADAREMITTERALLIED", StringComparison.OrdinalIgnoreCase) &&
                     !nodeText.Equals("RADAREMITTERALLIED", StringComparison.OrdinalIgnoreCase))
                LoadRadarEmitterAlliedFromDbByName(nodeText);
            else if (nodeText.StartsWith("COMEMITTER", StringComparison.OrdinalIgnoreCase) &&
                     !nodeText.Equals("COMEMITTER", StringComparison.OrdinalIgnoreCase))
                ShowCommEmitterPropertiesForm(nodeText);
            else if (nodeText.StartsWith("RADAR", StringComparison.OrdinalIgnoreCase) &&
                     !nodeText.Equals("RADAR", StringComparison.OrdinalIgnoreCase))
                ShowRadarPropertiesForm(nodeText);
            else
                ShowEmptyForm();
        }
    }
}