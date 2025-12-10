using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.Entity.Entities.Concrete;

namespace RuyaOptik.DataAccess.Context
{
    public class RuyaOptikDbContext : IdentityDbContext<IdentityUser>
    {
        public RuyaOptikDbContext(DbContextOptions<RuyaOptikDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
    }
}
