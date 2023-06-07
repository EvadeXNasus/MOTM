using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MOTM.Models;

namespace MOTM.Pages
{
    [Authorize(Roles = "Customer,Admin")]
    public class ProfileOverviewModel : PageModel
    {
        private MOTMContext _db;
        private UserManager<AppUser> _userManager;
        public IList<Service> Services { get; set; }
        public IList<Order> PendingOrders { get; set; }
        public Customer Customer { get; set; }
        public short Cost { get; set; } = 0;

        public ProfileOverviewModel (MOTMContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public void OnGet()
        {
            Services = _db.Services.FromSql($"SELECT * FROM Services").ToList();
            PendingOrders = _db.Orders.FromSql($"SELECT * FROM Orders WHERE Fulfilled = 0 AND CustomerId = {_userManager.GetUserId(User)}").ToList();
            PendingOrders = PendingOrders;
            Customer = _db.Customers.Find(_userManager.GetUserId(User));
        }
    }
}
