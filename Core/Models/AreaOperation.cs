
namespace EWTSS_DESKTOP.Core.Models
{
 public class AreaOperation : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Altitude { get; set; }

        public int ScenarioId { get; set; }
        public Scenario Scenario { get; set; }

        public ICollection<AreaOperationPolygon> Polygons { get; set; } = new List<AreaOperationPolygon>();
        public ICollection<ScenarioLine> Lines { get; set; } = new List<ScenarioLine>();
    }
}