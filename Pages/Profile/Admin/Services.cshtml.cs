using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MOTM.Models;

namespace MOTM.Pages.Profile.Admin
{
    [Authorize(Roles = "Admin")]
    public class ServicesModel : PageModel
    {
        public MOTMContext _db { get; set; }
        public ServicesModel(MOTMContext db)
        {
            _db = db;
        }
        [BindProperty]
        public IList<Service> ActiveServices { get; set; }
        [BindProperty]
        public IList<Service> InactiveServices { get; set; }

        public void OnGet()
        {
            ActiveServices = _db.Services.FromSql($"SELECT * FROM Services WHERE Active = 1 ORDER BY SortID").ToList();
            InactiveServices = _db.Services.FromSql($"SELECT * FROM Services WHERE Active = 0 ORDER BY SortID").ToList();
        }

        public async Task<IActionResult> OnPostDeactivateAsync(byte ServiceID)
        {
            Service Service = await _db.Services.FindAsync(ServiceID);
            Service.Active = false;
            _db.Attach(Service).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Service could not be deactivated", e);
            }
            return RedirectToPage("/Profile/Admin/Services");
        }

        public async Task<IActionResult> OnPostActivateAsync(byte ServiceID)
        {
            Service Service = await _db.Services.FindAsync(ServiceID);
            Service.Active = true;
            _db.Attach(Service).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Service could not be activated", e);
            }
            return RedirectToPage("/Profile/Admin/Services");
        }
    }
}
