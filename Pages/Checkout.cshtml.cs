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
        public IList<Service> Services { get; set; }
        [BindProperty]
        public long Choices { get; set; }
        /*[BindProperty]*/
        public IList<BookedSlot> BookedDays { get; set; }
        /*[BindProperty]*/
        public IList<DateTime> BookableDays { get; set; } = new List<DateTime>();
        [BindProperty]
        public string InputDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        [BindProperty]
        public string InputHour { get; set; } = "00";
        [BindProperty]
        public string InputMinute { get; set; } = "00";
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
            BookedDays = _db.BookedSlots.FromSql(
                $@"SELECT TimeSlot FROM Orders WHERE TimeSlot > {DateTime.Now.AddDays(6).ToString("yyyy-MM-dd")}
                AND TimeSlot < {DateTime.Today.AddDays(29).ToString("yyyy-MM-dd")}"
                ).ToList();
            foreach (var Service in Services)
            {
                if (Choices % Service.ID == 0)
                {
                    Cost += Service.Price;
                }
            }
            for (byte i = 0 ; i < 21 ; i++)
            {
                if (!BookedDays.Any(BookedDate => BookedDate.BookedDay == DateTime.Today.AddDays(i + 7)))
                {
                BookableDays.Add(DateTime.Today.AddDays(i + 7));
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Order order = new Order
            {
                CustomerID = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User)),
                Services = Choices,
                TimeSlot = DateTime.Parse(InputDate + "T" + InputHour + ":" + InputMinute),
                OrderedTime = DateTime.Now,
                Fulfilled = false
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Profile/ProfileOverview");
        }
    }
}
