namespace EWTSS_DESKTOP.Models
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }

        public int FeatureId { get; set; }

        public bool IsPermit { get; set; }

        public int UserId { get; set; }

        public Role Role { get; set; }

        public Feature Feature { get; set; }
    }
}