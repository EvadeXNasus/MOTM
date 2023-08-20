using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MOTM.Models;

namespace MOTM.Pages
{
    [Authorize]
    public class OrderSummaryModel : PageModel
    {
        private MOTMContext _db;
        private UserManager<AppUser> _userManager;
        public BookingDay[] BookingDaysInRange { get; set; } = new BookingDay[21];
        public IList<Order> OrdersInBookableRange { get; set; }
        [BindProperty]
        public string Input { get; set; } = "";
        public IList<Service> Services { get; set; }
        [BindProperty]
        public long Choices { get; set; }
        [BindProperty]
        public bool Snagged { get; set; }
        public short Cost { get; set; }
        [BindProperty]
        public short Duration { get; set; } = 0;
        [BindProperty]
        public bool Tomfoolery { get; set; } = false;
        public OrderSummaryModel(MOTMContext db, UserManager<AppUser> um)
        {
            _db = db;
            _userManager = um;
        }
        public void OnGet(string Handler)
        {
            Choices = long.Parse(Handler.Split(";")[0]);
            Snagged = bool.Parse(Handler.Split(";")[1]);
            Services = _db.Services.FromSql($"SELECT * FROM Services").ToList();
            foreach (Service Service in Services)
            {
                if (Choices % Service.ID == 0)
                {
                    Cost += Service.Price;
                    Duration += Service.Duration;
                }
            }
            if (Duration > 120 || Duration == 0 || Choices <= 0)
            {
                Tomfoolery = true;
            }
            OrdersInBookableRange = _db.Orders.FromSql(
                $@"SELECT * FROM Orders WHERE TimeSlot > {DateTime.Now.AddDays(6).ToString("yyyy-MM-dd")}
                AND TimeSlot < {DateTime.Today.AddDays(29).ToString("yyyy-MM-dd")}"
                ).ToList();

            for (int i = 0; i < 21; i++)
            {
                BookingDaysInRange[i] = new BookingDay
                {
                    TimeSlots = new DateTime[]
                    {
                        DateTime.Today.AddDays(i + 7).AddHours(9),
                        DateTime.Today.AddDays(i + 7).AddHours(12),
                        DateTime.Today.AddDays(i + 7).AddHours(15),
                        DateTime.Today.AddDays(i + 7).AddHours(18)
                    },
                    SlotsAreBooked = new bool[]
                    {
                        OrdersInBookableRange.Any(Order => Order.TimeSlot == DateTime.Today.AddDays(i + 7).AddHours(9)),
                        OrdersInBookableRange.Any(Order => Order.TimeSlot == DateTime.Today.AddDays(i + 7).AddHours(12)),
                        OrdersInBookableRange.Any(Order => Order.TimeSlot == DateTime.Today.AddDays(i + 7).AddHours(15)),
                        OrdersInBookableRange.Any(Order => Order.TimeSlot == DateTime.Today.AddDays(i + 7).AddHours(18))
                    }
                };
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Tomfoolery)
            {
                return RedirectToPage("Profile/AccessDenied");
            }
            if (DateTime.Parse(Input).Date > DateTime.Today.AddDays(6) && DateTime.Parse(Input).Date < DateTime.Today.AddDays(28))
            {
                Order Order = new Order
                {
                    CustomerID = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User)),
                    Services = Choices,
                    TimeSlot = DateTime.Parse(Input),
                    OrderedTime = DateTime.Now,
                    Fulfilled = false,
                    Paid = false
                };
                if (!_db.Orders.Any(_Order => _Order.TimeSlot == Order.TimeSlot))
                {
                    _db.Orders.Add(Order);
                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Profile/ProfileOverview");
                }
                else
                {
                    Snagged = true;
                    string Handler = Choices.ToString() + ";" + Snagged.ToString();
                    return RedirectToPage("OrderSummary", Handler);
                }
            }
            else
            {
                return RedirectToPage("/Profile/AccessDenied");
            }
        }
    }
}