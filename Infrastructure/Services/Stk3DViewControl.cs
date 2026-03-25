using System;
using System.Windows.Forms;

using System;

namespace EWTSS_DESKTOP.Infrastructure.Services
{
    public partial class Stk3DViewControl : System.Windows.Forms.UserControl
    {
        public Stk3DViewControl()
        {
            InitializeComponent();
        }

        public void CreateScenario(string scenarioName)
        {
            if (string.IsNullOrWhiteSpace(scenarioName))
                return;

            string safeName = scenarioName.Replace("\"", "").Trim();
            axAgUiAxVOCntrl1.Application.ExecuteCommand($"New / Scenario {safeName}");
        }
    }
}