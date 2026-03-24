using EWTSS_DESKTOP.Core.Enums;
using EWTSS_DESKTOP.Core.Models;

namespace EWTSS_DESKTOP.Core.Interfaces
{
    public interface ILogManagementRepository
    {
        Task<List<LogManagement>> GetAllAsync();
        Task<List<LogManagement>> GetByLogNameAsync(LogNameEnum logName);
        Task<List<LogManagement>> SearchAsync(string searchText, LogNameEnum? logName = null);
        Task AddAsync(LogManagement log);
    }
}