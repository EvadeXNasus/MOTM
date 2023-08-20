using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MOTM.Models;

namespace MOTM.Pages.Profile.Admin
{
    [Authorize(Roles = "Admin")]
    public class AllOrdersModel : PageModel
    {
        public MOTMContext _db { get; set; }
        public AllOrdersModel(MOTMContext db)
        {
            _db = db;
        }
        [BindProperty]
        public IList<CustomerOrder> CustomersOrders { get; set; }
        [BindProperty]
        public IList<Service> Services { get; set; }
        [BindProperty]
        public SearchFilter Input { get; set; }

        public void OnGet()
        {
            Services = _db.Services.FromSql($"SELECT * FROM Services").ToList();
            CustomersOrders = _db.CustomersOrders.FromSql(
                $@"SELECT
                Id AS CustomerId, FirstName, LastName, FirstAddressLine, SecondAddressLine, ThirdAddressLine, PostTown, PostCode, EMail, PhoneNumber,
                Services, OrderedTime, TimeSlot, Fulfilled, Paid
                FROM Orders INNER JOIN Customers ON Orders.CustomerID = Customers.Id"
                ).ToList();
        }

        public IActionResult OnPostFilter()
        {
            /* Sets these strings' values to empty if HTML Form inputs are left empty, as SQL parameters can't be null for the SELECT WHERE LIKE query to work */
            Input.FirstName = Input.FirstName == null ? "" : Input.FirstName;
            Input.LastName = Input.LastName == null ? "" : Input.LastName;

            /* Strings solely used for converting the DateTime inputs to FromSql-compatible parameters */
            string pOrderedMonth = Input.OrderedMonth.ToString("yyyy-MM") == "0001-01" ? "" : Input.OrderedMonth.ToString("yyyy-MM");
            string pBookedMonth = Input.BookedMonth.ToString("yyyy-MM") == "0001-01" ? "" : Input.BookedMonth.ToString("yyyy-MM");

            Services = _db.Services.FromSql($"SELECT * FROM Services").ToList();
            CustomersOrders = _db.CustomersOrders.FromSql(
                $@"SELECT
                Id AS CustomerId, FirstName, LastName, FirstAddressLine, SecondAddressLine, ThirdAddressLine, PostTown, PostCode, EMail, PhoneNumber, 
                Services, OrderedTime, TimeSlot, Fulfilled, Paid
                FROM Orders INNER JOIN Customers ON Orders.CustomerID = Customers.Id 
                WHERE FirstName LIKE '%' + {Input.FirstName} + '%' AND LastName LIKE '%' + {Input.LastName} + '%'
                AND OrderedTime LIKE '%' + {pOrderedMonth} + '%' AND TimeSlot LIKE '%' + {pBookedMonth} + '%'"
                ).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostFulfilAsync(string CustomerId, string OrderedTime)
        {
            DateTime TimeSlot = DateTime.Parse(OrderedTime);
            Order Order = await _db.Orders.FindAsync(CustomerId, TimeSlot);
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
            return RedirectToPage("/Profile/Admin/AllOrders");
        }

        public async Task<IActionResult> OnPostUndoFulfilmentAsync(string CustomerId, string OrderedTime)
        {
            DateTime TimeSlot = DateTime.Parse(OrderedTime);
            Order Order = await _db.Orders.FindAsync(CustomerId, TimeSlot);
            Order.Fulfilled = false;
            _db.Attach(Order).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Order could not be updated", e);
            }
            return RedirectToPage("/Profile/Admin/AllOrders");
        }

        public async Task<IActionResult> OnPostConfirmPaymentAsync(string CustomerId, string OrderedTime)
        {
            DateTime TimeSlot = DateTime.Parse(OrderedTime);
            Order Order = await _db.Orders.FindAsync(CustomerId, TimeSlot);
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
            return RedirectToPage("/Profile/Admin/AllOrders");
        }

        public async Task<IActionResult> OnPostUndoPaymentConfirmationAsync(string CustomerId, string OrderedTime)
        {
            DateTime TimeSlot = DateTime.Parse(OrderedTime);
            Order Order = await _db.Orders.FindAsync(CustomerId, TimeSlot);
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
            return RedirectToPage("/Profile/Admin/AllOrders");
        }

        public async Task<IActionResult> OnPostDeleteAsync(string CustomerId, string OrderedTime)
        {
            DateTime TimeSlot = DateTime.Parse(OrderedTime);
            Order Order = await _db.Orders.FindAsync(CustomerId, TimeSlot);
            _db.Remove<Order>(Order).State = EntityState.Deleted;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Order could not be deleted", e);
            }
            return RedirectToPage("/Profile/Admin/AllOrders");
        }
    }
}
