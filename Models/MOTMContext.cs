using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOTM.Models
{
    public class MOTMContext : IdentityDbContext<AppUser>
    {
        public MOTMContext (DbContextOptions<MOTMContext> options) : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        [NotMapped]
        public DbSet<CustomerOrder> CustomersOrders { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Order>().HasKey(t => new { t.CustomerID, t.TimeSlot });
            builder.Entity<Service>().ToTable("Services");
            builder.Entity<Customer>().ToTable("Customers");
            builder.Entity<Order>().ToTable("Orders");
            builder.Entity<CustomerOrder>().HasKey(t => new { t.CustomerId, t.TimeSlot});
            builder.Entity<CustomerOrder>().ToTable(t => t.ExcludeFromMigrations());
        }
    }
}