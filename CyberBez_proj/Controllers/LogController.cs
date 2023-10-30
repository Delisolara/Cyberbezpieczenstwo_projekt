using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CyberBez_proj.Controllers
{
    public class LogController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public LogController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
