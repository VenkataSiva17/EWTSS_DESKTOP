using System.Collections.Generic;

namespace EWTSS_DESKTOP.Core.Models
{
    public class Feature : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<RolePermission> Permissions { get; set; }
    }
}