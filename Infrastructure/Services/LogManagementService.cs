using EWTSS_DESKTOP.Core.Enums;
using EWTSS_DESKTOP.Core.Interfaces;
using EWTSS_DESKTOP.Core.Models;

namespace EWTSS_DESKTOP.Infrastructure.Services
{
    public class LogManagementService
    {
        private readonly ILogManagementRepository _logRepository;

        public LogManagementService(ILogManagementRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<List<LogManagement>> GetAllLogsAsync()
        {
            return await _logRepository.GetAllAsync();
        }

        public async Task<List<LogManagement>> GetLogsByCategoryAsync(LogNameEnum logName)
        {
            return await _logRepository.GetByLogNameAsync(logName);
        }

        public async Task<List<LogManagement>> SearchLogsAsync(string searchText, LogNameEnum? logName = null)
        {
            return await _logRepository.SearchAsync(searchText, logName);
        }

        public async Task AddLogAsync(
            int? userId,
            string userName,
            LogNameEnum logName,
            string logType,
            string module,
            string functionName,
            string message)
        {
            var log = new LogManagement
            {
                
                LogName = logName,
                LogType = logType,
                Module = module,
                FunctionName = functionName,
                Message = message,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                IsActive = true
            };

            await _logRepository.AddAsync(log);
        }
    }
}