using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EWTSS_DESKTOP.Core.Interfaces;
using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Infrastructure.Data;

namespace EWTSS_DESKTOP.Infrastructure.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly AppDbContext _context;

        public UserManagementRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() > 0;
        }

        public User? GetByUserName(string userName)
        {
            return _context.Users
                           .Include(u => u.Role)
                           .FirstOrDefault(u => u.UserName == userName);
        }

        public User? GetById(int id)
        {
            return _context.Users
                           .Include(u => u.Role)
                           .FirstOrDefault(u => u.Id == id && u.IsActive);
        }

        public List<User> GetAllUsers()
        {
            return _context.Users
                           .Include(u => u.Role)
                           .Where(u => u.IsActive)
                           .OrderBy(u => u.Id)
                           .ToList();
        }

        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteUser(User user)
        {
            user.IsActive = false;
            user.UpdatedOn = System.DateTime.Now;

            _context.Users.Update(user);
            return _context.SaveChanges() > 0;
        }
    }
}