using System.Windows;

namespace EWTSS_DESKTOP.Presentation.Views.Scenario
{
    public partial class CreateScenarioWindow : Window
    {
        public string ScenarioName => TxtName.Text.Trim();
        public string ScenarioDescription => TxtDescription.Text.Trim();

        public CreateScenarioWindow()
        {
            InitializeComponent();
        }

        private void TxtDescription_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int remaining = 256 - TxtDescription.Text.Length;
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
                System.Windows.MessageBox.Show("Scenario name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}