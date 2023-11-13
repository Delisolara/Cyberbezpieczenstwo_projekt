using Microsoft.AspNetCore.Mvc;

namespace CyberBez_proj.Controllers
{
    public class LoremController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
