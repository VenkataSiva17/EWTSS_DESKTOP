
namespace EWTSS_DESKTOP.Core.Models
{
    public class AreaOperationPolygon : BaseEntity
        {
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Altitude { get; set; }

            public int AreaOperationId { get; set; }
            public AreaOperation AreaOperation { get; set; }
        }
}