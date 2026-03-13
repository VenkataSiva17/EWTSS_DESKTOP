namespace EWTSS_DESKTOP.Core.Models
{
    public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int CreatedBy { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }

    public ICollection<TrScenario> Scenarios { get; set; }
}
}