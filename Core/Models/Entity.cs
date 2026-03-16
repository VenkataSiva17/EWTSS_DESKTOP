
namespace EWTSS_DESKTOP.Core.Models
{
public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double? StartFrequencyValue { get; set; }
        public double? StopFrequencyValue { get; set; }

        public string AntennaType { get; set; }
        public string Polarization { get; set; }
        public int? AntennaHeight { get; set; }
        public string ScanType { get; set; }

        public int CcId { get; set; }
        public EntityType EntityType { get; set; }

        public Cc Cc { get; set; }

        public ICollection<EntityPolygon> Polygons { get; set; } = new List<EntityPolygon>();
    }
}