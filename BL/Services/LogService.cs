using BL.Models;
using Microsoft.EntityFrameworkCore;

namespace BL.Services
{

    public class LogService 
    {
        private readonly DatabaseContext _databaseContext;
        public LogService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CreateLog(int level, string message)
        {

            var newLog = new Log
            {
                Level = level,
                Message = message
            };

            _databaseContext.Logs.Add(newLog);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Log>> GetAllLogs()
        {
            return await _databaseContext.Logs.ToListAsync();
        }
    }
}
