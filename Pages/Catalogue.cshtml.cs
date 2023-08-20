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
        public bool Empty;
        public bool Overloaded;
        public long Choices = 1;
        public short Duration = 0;
        [BindProperty]
        public bool[] Selected { get; set; }
        public CatalogueModel(MOTMContext db, SignInManager<AppUser> sm)
        {
            _db = db;
            _signInManager = sm;
        }

        public void OnGet(string Handler)
        {
            Services = _db.Services.FromSqlRaw("SELECT * FROM Services WHERE Active = 1 ORDER BY SortID").ToList();
            Selected = new bool[Services.Count];
            Empty = bool.Parse(Handler.Split(";")[0]);
            Overloaded = bool.Parse(Handler.Split(";")[1]);
        }

        public IActionResult OnPost(long Choices)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("Login");
            }
            Services = _db.Services.FromSqlRaw("SELECT * FROM Services WHERE Active = 1 ORDER BY SortID").ToList();
            for (int j = 0; j < Selected.Length; j++)
            {
                if (Selected[j])
                {
                    Choices *= Services[j].ID;
                    Duration += Services[j].Duration;
                }
            }
            if (Choices == 1)
            {
                Empty = true;
                return RedirectToPage("Catalogue", Empty.ToString() + ";False");
            }
            if (Duration > 120)
            {
                Overloaded = true;
                return RedirectToPage("Catalogue", "False;" + Overloaded.ToString());
            }
            string Handler = Choices.ToString() + ";False";
            return RedirectToPage("OrderSummary", Handler);
        }
    }
}
