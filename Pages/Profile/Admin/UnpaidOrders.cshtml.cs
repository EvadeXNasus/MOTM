using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MOTM.Models;

namespace MOTM.Pages.Profile.Admin
{
    [Authorize(Roles = "Admin")]
    public class UnpaidOrdersModel : PageModel
    {
        public MOTMContext _db { get; set; }
        public UnpaidOrdersModel(MOTMContext db)
        {
            _db = db;
        }
        [BindProperty]
        public IList<CustomerOrder> TodaysOrders { get; set; }
        [BindProperty]
        public IList<CustomerOrder> OverdueOrders { get; set; }
        [BindProperty]
        public IList<CustomerOrder> UpcomingOrders { get; set; }
        [BindProperty]
        public IList<Service> Services { get; set; }

        public void OnGet()
        {
            Services = _db.Services.FromSql($"SELECT * FROM Services").ToList();
            TodaysOrders = _db.CustomersOrders.FromSql(
                $@"SELECT
                Id AS CustomerId, FirstName, LastName, FirstAddressLine, SecondAddressLine, ThirdAddressLine, PostTown, PostCode, EMail, PhoneNumber,
                Services, OrderedTime, TimeSlot, Fulfilled, Paid
                FROM Orders INNER JOIN Customers ON Orders.CustomerID = Customers.Id
                WHERE Paid = 0 AND TimeSlot LIKE '{DateTime.Now.ToString("yyyy-MM-dd")}'"
                ).ToList();
            OverdueOrders = _db.CustomersOrders.FromSql(
                $@"SELECT
                Id AS CustomerId, FirstName, LastName, FirstAddressLine, SecondAddressLine, ThirdAddressLine, PostTown, PostCode, EMail, PhoneNumber,
                Services, OrderedTime, TimeSlot, Fulfilled, Paid
                FROM Orders INNER JOIN Customers ON Orders.CustomerID = Customers.Id
                WHERE Paid = 0 AND TimeSlot NOT LIKE '{DateTime.Now.ToString("yyyy-MM-dd")}' AND TimeSlot < CURRENT_TIMESTAMP"
                ).ToList();
            UpcomingOrders = _db.CustomersOrders.FromSql(
                $@"SELECT
                Id AS CustomerId, FirstName, LastName, FirstAddressLine, SecondAddressLine, ThirdAddressLine, PostTown, PostCode, EMail, PhoneNumber,
                Services, OrderedTime, TimeSlot, Fulfilled, Paid
                FROM Orders INNER JOIN Customers ON Orders.CustomerID = Customers.Id
                WHERE Paid = 0 AND TimeSlot NOT LIKE '{DateTime.Now.ToString("yyyy-MM-dd")}' AND TimeSlot > CURRENT_TIMESTAMP"
                ).ToList();
        }

        public async Task<IActionResult> OnPostFulfilAsync(string CustomerID, string TimeSlot)
        {
            DateTime _TimeSlot = DateTime.Parse(TimeSlot);
            Order Order = await _db.Orders.FindAsync(CustomerID, _TimeSlot);
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
            return RedirectToPage("/Profile/Admin/UnpaidOrders");
        }

        public async Task<IActionResult> OnPostConfirmPaymentAsync(string CustomerID, string TimeSlot)
        {
            DateTime _TimeSlot = DateTime.Parse(TimeSlot);
            Order Order = await _db.Orders.FindAsync(CustomerID, _TimeSlot);
            Order.Paid = true;
            _db.Attach(Order).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Order could not be updated", e);
            }
            return RedirectToPage("/Profile/Admin/UnpaidOrders");
        }

        public async Task<IActionResult> OnPostUndoPaymentConfirmationAsync(string CustomerID, string TimeSlot)
        {
            DateTime _TimeSlot = DateTime.Parse(TimeSlot);
            Order Order = await _db.Orders.FindAsync(CustomerID, _TimeSlot);
            Order.Paid = false;
            _db.Attach(Order).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Order could not be updated", e);
            }
            return RedirectToPage("/Profile/Admin/UnpaidOrders");
        }

        public async Task<IActionResult> OnPostDeleteAsync(string CustomerId, string OrderedTime)
        {
            DateTime TimeSlot = DateTime.Parse(OrderedTime);
            var Order = await _db.Orders.FindAsync(CustomerId, TimeSlot);
            _db.Remove<Order>(Order).State = EntityState.Deleted;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Order could not be deleted", e);
            }
            return RedirectToPage("/Profile/Admin/UnpaidOrders");
        }
    }
}
