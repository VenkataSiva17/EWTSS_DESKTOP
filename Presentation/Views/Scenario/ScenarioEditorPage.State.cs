using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioEditorPage
    {
        private int? _selectedCcId;
        private int? _selectedEntityId;
        private int? _selectedEmitterId;
        private int? _selectedAreaOperationId;
        private int? _selectedRadarEmitterId;
        private int? _selectedCommEmitterId;

        private void SetAreaOperationButtonState(bool isUpdate)
        {
            CreateAreaOperationButton.Content = isUpdate ? "UPDATE" : "CREATE";
        }

        private void SetRdfsButtonState(bool isUpdate)
        {
            CreateRdfsButton.Content = isUpdate ? "UPDATE" : "CREATE";
        }

        private static bool TryParseDouble(string? value, out double result)
        {
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                   || double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result);
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}