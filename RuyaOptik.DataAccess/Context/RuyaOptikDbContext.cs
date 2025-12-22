using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.Entity.Identity;
using RuyaOptik.Entity.Concrete;
using RuyaOptik.Entity.Enums;
using RuyaOptik.DataAccess.Repositories.Configuration;
namespace RuyaOptik.DataAccess.Context
{
    public class RuyaOptikDbContext : IdentityDbContext<AspUser,AspRole,string>
    {
        public RuyaOptikDbContext(DbContextOptions<RuyaOptikDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

        }
        public DbSet<AspUser> users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

    }
}
