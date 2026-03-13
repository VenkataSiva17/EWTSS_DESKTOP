using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Infrastructure.Repositories;

namespace EWTSS_DESKTOP.Infrastructure.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;

        public UserService(UserRepository repo)
        {
            _repo = repo;
        }

        public User ValidateUser(string username, string password)
        {
            return _repo.GetUser(username, password);
        }
    }
}