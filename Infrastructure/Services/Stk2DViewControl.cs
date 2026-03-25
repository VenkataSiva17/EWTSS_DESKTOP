using System;
using System.Windows.Forms;

using System;

namespace EWTSS_DESKTOP.Infrastructure.Services
{
    public partial class Stk2DViewControl : System.Windows.Forms.UserControl
    {
        public Stk2DViewControl()
        {
            InitializeComponent();
        }

        public void CreateScenario(string scenarioName)
        {
            if (string.IsNullOrWhiteSpace(scenarioName))
                return;

            string safeName = scenarioName.Replace("\"", "").Trim();
            axAgUiAx2DCntrl1.Application.ExecuteCommand($"New / Scenario {safeName}");
        }
    }
}