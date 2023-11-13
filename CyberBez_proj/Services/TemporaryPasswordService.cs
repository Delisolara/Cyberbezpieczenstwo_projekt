namespace CyberBez_proj.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class TemporaryPasswordService
{
    private readonly UserManager<IdentityUser> _userManager;

    public TemporaryPasswordService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> GenerateTemporaryPassword(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            // Obsłuż błąd - użytkownik nie istnieje
            return null;
        }

        // Wygeneruj jednorazowe hasło
        var temporaryPassword = await _userManager.GenerateUserTokenAsync(user, "Default", "TemporaryPassword");

        return temporaryPassword;
    }
}