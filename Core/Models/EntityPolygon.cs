namespace EWTSS_DESKTOP.Core.Models
{
    public class EntityPolygon : BaseEntity
        {
            public string Latitude { get; set; }
            public string Longitude { get; set; }

            public int EntityId { get; set; }
            public Entity Entity { get; set; }
        }
}