using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MOTM.Models;

namespace MOTM.Pages
{
    [Authorize]
    public class ContactDetailsModel : PageModel
    {
        public MOTMContext _db { get; set; }
        private UserManager<AppUser> _userManager { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }

        public ContactDetailsModel (MOTMContext db, UserManager<AppUser> um)
        {
            _db = db;
            _userManager = um;
        }

        public void OnGet()
        {
            Customer = _db.Customers.Find(_userManager.GetUserId(User));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _db.Attach(Customer).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Profile could not be updated ", e);
            }
            return RedirectToPage("ProfileOverview");
        }
    }
}