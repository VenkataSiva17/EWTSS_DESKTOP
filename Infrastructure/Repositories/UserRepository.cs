using System.Linq;
using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Infrastructure.Data;

namespace EWTSS_DESKTOP.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public User GetUser(string username, string password)
        {
            return _db.Users
                .FirstOrDefault(u =>
                    u.UserName == username &&
                    u.Password == password &&
                    u.IsActive == true);
        }
    }
}