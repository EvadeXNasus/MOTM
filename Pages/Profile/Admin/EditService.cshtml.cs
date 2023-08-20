using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MOTM.Models;

namespace MOTM.Pages.Profile.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditServiceModel : PageModel
    {
        public MOTMContext _db { get; set; }
        public EditServiceModel(MOTMContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Service Service { get; set; }

        public async void OnGetAsync(string ServiceID)
        {
            Service = await _db.FindAsync<Service>(byte.Parse(ServiceID));
        }

        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            _db.Attach(Service).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Service could not be updated", e);
            }
            return RedirectToPage("/Profile/Admin/Services");
        }
    }
}
