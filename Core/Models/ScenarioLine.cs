
namespace EWTSS_DESKTOP.Core.Models
{
public class ScenarioLine : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int AreaOperationId { get; set; }

       public LineType LineType { get; set; }

        public AreaOperation AreaOperation { get; set; }

        public ICollection<Cc> Ccs { get; set; } = new List<Cc>();
        public ICollection<Emitter> Emitters { get; set; } = new List<Emitter>();
    }
}