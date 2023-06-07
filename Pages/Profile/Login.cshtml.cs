using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MOTM.Models;

namespace MOTM.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public SignIn Input { get; set; }
        public readonly SignInManager<AppUser> _signInManager;

        public LoginModel(SignInManager<AppUser> sm)
        {
            _signInManager = sm;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.EMail, Input.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToPage("ProfileOverview");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
                }
            }
            return Page();
        }
    }
}
