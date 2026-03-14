using System;
using AGI.STKObjects;
using AGI.STKUtil;

namespace EWTSS_DESKTOP.Infrastructure.Services
{
    public class StkEngineService : IDisposable
    {
        private AgStkObjectRoot _root;

        public IAgStkObjectRoot Root => _root;

        public void Initialize()
        {
            if (_root != null)
                return;

            _root = new AgStkObjectRoot();

            // Reset units and use UTCG for scenario times
            var units = _root.UnitPreferences;
            units.ResetUnits();
            units.SetCurrentUnit("DateFormat", "UTCG");
        }

        public void CreateScenario(string scenarioName, DateTime startUtc, TimeSpan duration)
        {
            Initialize();

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

        public void Dispose()
        {
            if (_root != null)
            {
                if (_root.CurrentScenario != null)
                    _root.CloseScenario();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(_root);
                _root = null;
            }
        }
    }
}