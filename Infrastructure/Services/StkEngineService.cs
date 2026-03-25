using System;
using System.Windows;
using AGI.STKObjects;
using AGI.STKUtil;
using AGI.STKX;

namespace EWTSS_DESKTOP.Infrastructure.Services
{
    public class StkEngineService : IDisposable
    {
        private AgStkObjectRoot _root;

        public IAgStkObjectRoot Root => (IAgStkObjectRoot)_root;

        /// <summary>
        /// Check whether STK Engine and required license is available
        /// </summary>
        public bool CheckLicense()
        {
            Console.WriteLine("Checking STK license...");

            AgSTKXApplication stkxApp = null;

            try
            {
                stkxApp = new AgSTKXApplication();

                // Check required feature (you can change this if needed)
                if (!stkxApp.IsFeatureAvailable(AgEFeatureCodes.eFeatureCodeGlobeControl))
                {
                    MessageBox.Show(
                        "Required STK license is not available.",
                        "License Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Stop
                    );

                    return false;
                }

                return true;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                // STK not installed
                if (ex.ErrorCode == unchecked((int)0x80040154))
                {
                    string errorMessage =
                        "Could not initialize STK Engine.\n\n" +
                        "Please ensure STK / STK Engine 64-bit is installed.";

                    MessageBox.Show(
                        errorMessage,
                        "STK Engine Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Stop
                    );

                    return false;
                }

                throw;
            }
            finally
            {
                if (stkxApp != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(stkxApp);
                    stkxApp = null;
                }
            }
        }

        /// <summary>
        /// Initialize STK Engine
        /// </summary>
        public void Initialize()
        {
            if (_root != null)
                return;

            // ✅ Ensure license before creating root
            if (!CheckLicense())
            {
                throw new Exception("STK License not available. Initialization failed.");
            }

            _root = new AgStkObjectRoot();

            // Set units
            var units = _root.UnitPreferences;
            units.ResetUnits();
            units.SetCurrentUnit("DateFormat", "UTCG");
        }

        /// <summary>
        /// Ensure STK is initialized
        /// </summary>
        private void EnsureInitialized()
        {
            if (_root == null)
                Initialize();
        }

        /// <summary>
        /// Create new scenario
        /// </summary>
        public void CreateScenario(string scenarioName, DateTime startUtc, TimeSpan duration)
        {
            EnsureInitialized();

            if (_root.CurrentScenario != null)
            {
                _root.CloseScenario();
            }

            _root.NewScenario(scenarioName);

            var scenario = (IAgScenario)_root.CurrentScenario;

            var stopUtc = startUtc.Add(duration);

            string startText = startUtc.ToString("dd MMM yyyy HH:mm:ss.00");
            string stopText = stopUtc.ToString("dd MMM yyyy HH:mm:ss.00");

            scenario.SetTimePeriod(startText, stopText);
            scenario.Epoch = startText;
        }

        /// <summary>
        /// Close current scenario
        /// </summary>
        public void CloseScenario()
        {
            if (_root?.CurrentScenario != null)
            {
                _root.CloseScenario();
            }
        }

        /// <summary>
        /// Dispose STK resources
        /// </summary>
        public void Dispose()
        {
            if (_root != null)
            {
                try
                {
                    if (_root.CurrentScenario != null)
                    {
                        _root.CloseScenario();
                    }
                }
                catch { }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(_root);
                _root = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}

// var stkService = new StkEngineService();

// stkService.CreateScenario(
//     "TestScenario",
//     DateTime.UtcNow,
//     TimeSpan.FromHours(2)
// );