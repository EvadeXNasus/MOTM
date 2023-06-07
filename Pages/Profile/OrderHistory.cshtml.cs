using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MOTM.Models;

namespace MOTM.Pages
{
    [Authorize]
    public class OrderHistoryModel : PageModel
    {
        private MOTMContext _db { get; set; }
        public UserManager<AppUser> _userManager { get; set; }
        public IList<Service> Services { get; set; }
        public IList<Order> Orders { get; set; }
        public short Cost { get; set; } = 0;
        [BindProperty]
        public bool _Searched { get; set; }
        [BindProperty]
        public DateTime SearchMonth { get; set; } = DateTime.Now;
        public OrderHistoryModel (MOTMContext db, UserManager<AppUser> um)
        {
            _db = db;
            _userManager = um;
        }
        public void OnGet(bool Searched)
        {
            _Searched = Searched;
            Services = _db.Services.FromSql($"SELECT * FROM Services").ToList();
            Orders = _db.Orders.FromSql($"SELECT * FROM Orders WHERE CustomerID = {_userManager.GetUserId(User)}" ).ToList();
        }

        public IActionResult OnPostSearch()
        {
            Services = _db.Services.FromSql($"SELECT * FROM Services").ToList();
            Orders = _db.Orders.FromSql($"SELECT * FROM Orders WHERE CustomerID = {_userManager.GetUserId(User)} AND TimeSlot LIKE '%' + {SearchMonth.ToString("yyyy-MM")} + '%'").ToList();
            _Searched = true;
            return Page();
        }
    }
}
