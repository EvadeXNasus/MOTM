using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MOTM.Models;

namespace MOTM.Pages
{
    [Authorize]
    public class OrderSummaryModel : PageModel
    {
        private MOTMContext _db;
        public IList<Service> Services { get; set; }
        public long Choices { get; set; }
        public short Cost { get; set; }
        public OrderSummaryModel(MOTMContext db)
        {
            _db = db;
        }
        public void OnGet(long sum)
        {
            Services = _db.Services.FromSqlRaw("SELECT * FROM Services").ToList();
            Choices = sum;
            foreach (var Service in Services)
            {
                if (Choices % Service.ID == 0)
                {
                    Cost += Service.Price;
                }
            }
        }
    }
}