using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EWTSS_DESKTOP.Core.Models
{
    public class User : BaseEntity
    {
        [NotMapped]
        public int SerialNo { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int CreatedBy { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Scenario> CreatedScenarios { get; set; } = new List<Scenario>();
    }
}