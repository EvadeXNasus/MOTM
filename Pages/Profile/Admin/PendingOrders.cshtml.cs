using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MOTM.Models;

namespace MOTM.Pages.Profile.Admin
{
    [Authorize(Roles = "Admin")]
    public class PendingOrdersModel : PageModel
    {
        public MOTMContext _db { get; set; }
        public PendingOrdersModel(MOTMContext db)
        {
            _db = db;
        }
        [BindProperty]
        public IList<Order> TodaysOrders { get; set; }
        [BindProperty]
        public IList<Order> OverdueOrders { get; set; }
        [BindProperty]
        public IList<Order> UpcomingOrders { get; set; }
        [BindProperty]
        public IList<Customer> Customers { get; set; }
        [BindProperty]
        public IList<Service> Services { get; set; }

        public void OnGet()
        {
            TodaysOrders = _db.Orders.FromSql($"SELECT * FROM Orders WHERE TimeSlot LIKE '{DateTime.Now.ToString("yyyy-MM-dd")}'").ToList();
            OverdueOrders = _db.Orders.FromSql($"SELECT * FROM Orders WHERE Fulfilled = 0 AND TimeSlot NOT LIKE '{DateTime.Now.ToString("yyyy-MM-dd")}' AND TimeSlot < CURRENT_TIMESTAMP").ToList();
            UpcomingOrders = _db.Orders.FromSql($"SELECT * FROM Orders WHERE Fulfilled = 0 AND TimeSlot NOT LIKE '{DateTime.Now.ToString("yyyy-MM-dd")}' AND TimeSlot > CURRENT_TIMESTAMP").ToList();
            Customers = _db.Customers.FromSql($"SELECT * FROM Customers").ToList();
            Services = _db.Services.FromSql($"SELECT * FROM Services").ToList();
        }

        public async Task<IActionResult> OnPostCompleteAsync(string CustomerID, string TimeSlot)
        {
            DateTime _TimeSlot = DateTime.Parse(TimeSlot);
            var Order =  await _db.Orders.FindAsync(CustomerID, _TimeSlot);
            Order.Fulfilled = true;
            _db.Attach(Order).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Order could not be updated", e);
            }
            return RedirectToPage("/Profile/Admin/PendingOrders");
        }
    }
}
