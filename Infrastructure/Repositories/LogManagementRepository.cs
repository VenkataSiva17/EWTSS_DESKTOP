using Microsoft.EntityFrameworkCore;
using EWTSS_DESKTOP.Core.Enums;
using EWTSS_DESKTOP.Core.Interfaces;
using EWTSS_DESKTOP.Core.Models;
using EWTSS_DESKTOP.Infrastructure.Data;

namespace EWTSS_DESKTOP.Infrastructure.Repositories
{
    public class LogManagementRepository : ILogManagementRepository
    {
        private readonly AppDbContext _context;

        public LogManagementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LogManagement>> GetAllAsync()
        {
            return await _context.LogManagements
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<List<LogManagement>> GetByLogNameAsync(LogNameEnum logName)
        {
            return await _context.LogManagements
                .Where(x => x.IsActive && x.LogName == logName)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task<List<LogManagement>> SearchAsync(string searchText, LogNameEnum? logName = null)
        {
            var query = _context.LogManagements
                .Where(x => x.IsActive)
                .AsQueryable();

            if (logName.HasValue)
                query = query.Where(x => x.LogName == logName.Value);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.ToLower();

                query = query.Where(x =>
                    (x.LogType != null && x.LogType.ToLower().Contains(searchText)) ||
                    (x.Module != null && x.Module.ToLower().Contains(searchText)) ||
                    (x.FunctionName != null && x.FunctionName.ToLower().Contains(searchText)) ||
                    (x.Message != null && x.Message.ToLower().Contains(searchText)) 
                    
                );
            }

            return await query
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();
        }

        public async Task AddAsync(LogManagement log)
        {
            await _context.LogManagements.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}