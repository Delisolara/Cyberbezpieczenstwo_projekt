using CyberBez_proj.Data;
using CyberBez_proj.Models;
using Microsoft.Identity.Client;
using NuGet.Packaging.Signing;

namespace CyberBez_proj.Services
{
    public interface ILogService
    {
        void Add(string actionexecutedBy, string userId, string action, bool isSuccessful);
        List<Log> GetAllLogs();
        List<Log> GetUserLogs(string userId);
    }
    public class LogService : ILogService
    {
        private readonly ApplicationDbContext _context;

        public LogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(string actionexecutedBy, string userId, string action, bool isSuccessful)
        {
            var log = new Log
            {
                UserID = userId,
                ActionExecutedBy = actionexecutedBy,
                IsSuccessful = isSuccessful,
                Action = action,
                Timestamp = DateTime.Now,
            };

            _context.logs.Add(log);
            _context.SaveChanges();
        }
            public List<Log> GetAllLogs()
            {
                return _context.logs.ToList();
            }

            public List<Log> GetUserLogs(string userId)
            {
                return _context.logs.Where(log => log.UserID == userId).ToList();
            }
        
       

    }
}
