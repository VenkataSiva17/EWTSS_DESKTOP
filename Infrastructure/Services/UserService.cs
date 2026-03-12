using EWTSS_DESKTOP.Models;
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

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _repo.GetUserAsync(username, password);
        }
    }
}