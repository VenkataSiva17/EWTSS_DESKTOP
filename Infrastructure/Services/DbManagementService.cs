using EWTSS_DESKTOP.Core.Interfaces;

namespace EWTSS_DESKTOP.Infrastructure.Services
{
    public class DbManagementService
    {
        private readonly IDbManagementRepository _repository;

        public DbManagementService(IDbManagementRepository repository)
        {
            _repository = repository;
        }

        public bool Backup(string path, out string message)
        {
            return _repository.BackupDatabase(path, out message);
        }

        public bool Import(string path, out string message)
        {
            return _repository.ImportDatabase(path, out message);
        }

        public bool Purge(out string message)
        {
            string[] tables = { "user", "role", "scenario","__efmigrationshistory","area_operation" };
            return _repository.PurgeDatabase(tables, out message);
        }
    }
}