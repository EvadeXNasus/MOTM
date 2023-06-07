using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MOTM.Models;

namespace MOTM.Pages
{
    [Authorize]
    public class LogoutConfirmationModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        
        public LogoutConfirmationModel(SignInManager<AppUser> sm)
        {
            _signInManager = sm;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
