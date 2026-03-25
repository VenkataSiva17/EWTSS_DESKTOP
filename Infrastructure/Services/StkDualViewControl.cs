using System;
using System.Windows.Forms;

namespace EWTSS_DESKTOP.Infrastructure.Services
{
    public partial class StkDualViewControl : UserControl
    {
        public StkDualViewControl()
        {
            InitializeComponent();
        }

        public void CreateVisualScenario(string scenarioName)
        {
            if (string.IsNullOrWhiteSpace(scenarioName))
                throw new ArgumentException("Scenario name is required.", nameof(scenarioName));

            string safeName = scenarioName.Replace("\"", "").Trim();

            // This is the same pattern the STK tutorial uses.
            axAgUiAx2DCntrl1.Application.ExecuteCommand($"New / Scenario {safeName}");
        }

        public void ZoomIn2D()
        {
            axAgUiAx2DCntrl1.ZoomIn();
        }

        public void ZoomOut2D()
        {
            axAgUiAx2DCntrl1.ZoomOut();
        }

        public void CloseVisualScenario()
        {
            try
            {
                axAgUiAx2DCntrl1.Application.ExecuteCommand("Unload / *");
            }
            catch
            {
                // ignore for first pass
            }
        }
    }
}