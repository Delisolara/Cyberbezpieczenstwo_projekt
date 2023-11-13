using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CyberBez_proj.Services;

public class TemporaryPasswordController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly TemporaryPasswordService _otpService;

    public TemporaryPasswordController(UserManager<IdentityUser> userManager, TemporaryPasswordService otpService)
    {
        _userManager = userManager;
        _otpService = otpService;
    }

    public IActionResult GenerateTemporaryPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GenerateTemporaryPassword(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Użytkownik o podanym identyfikatorze nie istnieje.");
            return View();
        }

        var temporaryPassword = await _otpService.GenerateTemporaryPassword(userId);

        if (temporaryPassword != null)
        {
            ViewBag.TemporaryPassword = temporaryPassword;
        }

        return View();
    }
}