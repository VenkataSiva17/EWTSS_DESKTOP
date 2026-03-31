using System;
using System.Windows;
using System.Windows.Controls;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class CreateScenarioWindow : Window
    {
        public string ScenarioName => TxtName.Text.Trim();
        public string ScenarioDescription => TxtDescription.Text.Trim();

        public CreateScenarioWindow()
        {
            InitializeComponent();

            CreateButton.IsEnabled = false;
            TxtRemaining.Text = "256 characters remaining";
        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateButton.IsEnabled = !string.IsNullOrWhiteSpace(TxtName.Text);
        }

        private void TxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            int remaining = Math.Max(0, 256 - TxtDescription.Text.Length);
            TxtRemaining.Text = $"{remaining} characters remaining";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                System.Windows.MessageBox.Show(
                    "Scenario name is required.",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtName.Focus();
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}