
namespace EWTSS_DESKTOP.Core.Models
{
public class Cc
    {
        public int Id { get; set; }
        public string CcName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public int LineId { get; set; }
        public ScenarioLine Line { get; set; }

        public ICollection<Entity> Entities { get; set; } = new List<Entity>();
    }
}