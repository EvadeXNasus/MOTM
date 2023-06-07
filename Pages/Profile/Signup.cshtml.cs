using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MOTM.Models;

namespace MOTM.Pages
{
    [AllowAnonymous]
    public class SignupModel : PageModel
    {
        [BindProperty]
        public Registration Input { get; set; }
        private MOTMContext _db;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public SignupModel(MOTMContext db, SignInManager<AppUser> sm, UserManager<AppUser> um)
        {
            _db = db;
            _signInManager = sm;
            _userManager = um;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = Input.EMail,
                    Email = Input.EMail,
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userManager.AddToRoleAsync(user, "Customer");
                    var customer = new Customer
                    {
                        Id = user.Id,
                        EMail = Input.EMail,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        PhoneNumber = Input.PhoneNumber,
                        FirstAddressLine = Input.FirstAddressLine,
                        SecondAddressLine = Input.SecondAddressLine,
                        ThirdAddressLine = Input.ThirdAddressLine,
                        PostTown = Input.PostTown,
                        PostCode = Input.PostCode
                    };
                    _db.Customers.Add(customer);
                    await _db.SaveChangesAsync();
                    return RedirectToPage("ProfileOverview");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
