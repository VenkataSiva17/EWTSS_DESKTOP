using System.Threading.Tasks;                  // Needed for Task<>
using Microsoft.EntityFrameworkCore;          // Needed for Include() and FirstOrDefaultAsync()
using EWTSS_DESKTOP.Models;                   // Needed for User
using EWTSS_DESKTOP.Data;      // Needed for AppDbContext

namespace EWTSS_DESKTOP.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) => _db = db;

        public async Task<User?> GetUserAsync(string username, string password)
        {
            // In production, use hashed passwords instead of plain text
            return await _db.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }
    }
}