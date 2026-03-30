using System.Collections.Generic;
using EWTSS_DESKTOP.Core.Models;

namespace EWTSS_DESKTOP.Core.Interfaces
{
    public interface IUserManagementRepository
    {
        bool CreateUser(User user);
        User? GetByUserName(string userName);
        List<User> GetAllUsers();

        User? GetById(int id);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
    }
}