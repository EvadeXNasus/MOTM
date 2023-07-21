using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MOTM.Models;
using System.Collections;

namespace MOTM.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private MOTMContext _db;
        private UserManager<AppUser> _userManager;
        [BindProperty]
        public long Choices { get; set; }
        public IList<Service> Services { get; set; }
        public DateTime[,] BookingSlotsInRange { get; set; } = new DateTime[21,5];
        public IList<Order> OrdersInBookableRange { get; set; }
        [BindProperty]
        public string Input { get; set; } = "";
        public short Cost { get; set; }
        public CheckoutModel(MOTMContext db, UserManager<AppUser> um)
        {
            _db = db;
            _userManager = um;
        }
        public void OnGet(long choices)
        {
            Choices = choices;
            Services = _db.Services.FromSql($"SELECT * FROM Services").ToList();
            for (byte i = 0; i < 21; i++)
            {
                BookingSlotsInRange[i, 0] = DateTime.Parse(DateTime.Today.AddDays(i + 7).ToString("yyyy-MM-dd") + "T" + "10:00");
                BookingSlotsInRange[i, 1] = DateTime.Parse(DateTime.Today.AddDays(i + 7).ToString("yyyy-MM-dd") + "T" + "12:00");
                BookingSlotsInRange[i, 2] = DateTime.Parse(DateTime.Today.AddDays(i + 7).ToString("yyyy-MM-dd") + "T" + "14:00");
                BookingSlotsInRange[i, 3] = DateTime.Parse(DateTime.Today.AddDays(i + 7).ToString("yyyy-MM-dd") + "T" + "16:00");
                BookingSlotsInRange[i, 4] = DateTime.Parse(DateTime.Today.AddDays(i + 7).ToString("yyyy-MM-dd") + "T" + "18:00");
            }
            OrdersInBookableRange = _db.Orders.FromSql(
                $@"SELECT * FROM Orders WHERE TimeSlot > {DateTime.Now.AddDays(6).ToString("yyyy-MM-dd")}
                AND TimeSlot < {DateTime.Today.AddDays(29).ToString("yyyy-MM-dd")}"
                ).ToList();
            foreach (var Service in Services)
            {
                if (Choices % Service.ID == 0)
                {
                    Cost += Service.Price;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Order order = new Order
            {
                CustomerID = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User)),
                Services = Choices,
                TimeSlot = DateTime.Parse(Input),
                OrderedTime = DateTime.Now,
                Fulfilled = false
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Profile/ProfileOverview");
        }
    }
}
