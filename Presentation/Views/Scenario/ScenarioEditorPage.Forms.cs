using System.Windows;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class ScenarioEditorPage
    {
        private void HideAllForms()
        {
            ScenarioDetailsBorder.Visibility = Visibility.Collapsed;
            AreaOfOperationBorder.Visibility = Visibility.Collapsed;
            CcPropertiesBorder.Visibility = Visibility.Collapsed;
            RdfsPropertiesBorder.Visibility = Visibility.Collapsed;
            JsvushfPropertiesBorder.Visibility = Visibility.Collapsed;
            CommEmitterAlliedPropertiesBorder.Visibility = Visibility.Collapsed;
            RadarEmitterAlliedPropertiesBorder.Visibility = Visibility.Collapsed;
            EmptyDetailsBorder.Visibility = Visibility.Collapsed;
            CommEmitterPropertiesBorder.Visibility = Visibility.Collapsed;
            RadarPropertiesBorder.Visibility = Visibility.Collapsed;
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
        }

        private void ShowJsvushfPropertiesForm(string name)
        {
            HideAllForms();
            JsvushfPropertiesBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;

            JsvushfTitleTextBlock.Text = $"{name} PROPERTIES";
            JsvushfNameTextBox.Text = name;
        }

        private void ShowCommEmitterAlliedPropertiesForm(string name)
        {
            HideAllForms();
            CommEmitterAlliedPropertiesBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;

            CommEmitterAlliedTitleTextBlock.Text = $"{name} PROPERTIES";
            CommEmitterNameTextBox.Text = name;
        }

        private void ShowRadarEmitterAlliedPropertiesForm(string name)
        {
            HideAllForms();
            RadarEmitterAlliedPropertiesBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;

            RadarEmitterAlliedTitleTextBlock.Text = $"{name} PROPERTIES";
            RadarEmitterNameTextBox.Text = name;
        }

        private void ShowCommEmitterPropertiesForm(string name)
        {
            HideAllForms();
            CommEmitterPropertiesBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;

            CommEmitterNameTextBox2.Text = name;
        }

        private void ShowRadarPropertiesForm(string name)
        {
            HideAllForms();
            RadarPropertiesBorder.Visibility = Visibility.Visible;
            DrsTableBorder.Visibility = Visibility.Visible;

            RadarNameTextBox2.Text = name;
        }
    }
}