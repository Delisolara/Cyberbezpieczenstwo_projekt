using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CyberBez_proj.Models;
using CyberBez_proj.Services;

namespace CyberBez_proj.Controllers
{
    public class LogViewController : Controller
    {
        private readonly ILogService _logService;
        public LogViewController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public IActionResult ViewAlllogs()
        {
            var logs = _logService.GetAllLogs();
            return View(logs);
        }

        //[HttpGet]
        //public IActionResult ViewUserLogs(string userId)
        //{
        //    var logs = _logService.GetUserLogs(userId);
        //    return View(logs);
        //}
 
 
    }
}
