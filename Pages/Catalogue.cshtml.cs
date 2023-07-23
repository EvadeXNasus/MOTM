using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MOTM.Models;

namespace MOTM.Pages
{
    [Authorize]
    public class CatalogueModel : PageModel
    {
        private MOTMContext _db;
        private SignInManager<AppUser> _signInManager;
        public IList<Service> Services { get; set; }
        public byte i = 0;
        public bool _empty;
        public long sum = 1;
        [BindProperty]
        public bool[] Selected { get; set; } = new bool[13];
        public CatalogueModel(MOTMContext db, SignInManager<AppUser> sm)
        {
            _db = db;
            _signInManager = sm;
        }

        public void OnGet(bool empty)
        {
            Services = _db.Services.FromSqlRaw("SELECT * FROM Services").ToList();
            _empty = empty;
        }

        public IActionResult OnPost(long sum)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("Login");
            }
            Services = _db.Services.FromSqlRaw("SELECT * FROM Services").ToList();
            for (int j = 0; j < Selected.Length; j++)
            {
                if (Selected[j])
                {
                    sum *= Services[j].ID;
                }
            }
            if (sum == 1)
            {
                _empty = true;
                return RedirectToPage("Catalogue", new {empty = _empty});
            }
            return RedirectToPage("OrderSummary", new {sum});
        }
    }
}
