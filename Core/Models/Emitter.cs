 
namespace EWTSS_DESKTOP.Core.Models
{
    public class Emitter : BaseEntity
    {
        public string Name { get; set; }
        public PlatformType PlatformType { get; set; }

        public string ModeType { get; set; }
        public double? PowerTransmitted { get; set; }
        public double? StartFrequencyValue { get; set; }
        public double? StopFrequencyValue { get; set; }

        public double? HopPeriodValue { get; set; }
        public string HopPeriodUnit { get; set; }

        public double? HopInterPeriodValue { get; set; }
        public string HopInterPeriodUnit { get; set; }

        public double? Bandwidth { get; set; }
        public string ModulationType { get; set; }
        public string PatternType { get; set; }
        public double? ScanRate { get; set; }

        public string AntennaType { get; set; }
        public double? Gain { get; set; }
        public string Polarization { get; set; }

        public int LineId { get; set; }
        public string EmitterType { get; set; }
        public EmitterKeyName Type { get; set; }

        public ScenarioLine Line { get; set; }
    }
}