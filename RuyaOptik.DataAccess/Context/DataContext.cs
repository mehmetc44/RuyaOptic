using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.Entity.Entities;

namespace RuyaOptik.DataAccess.Context
{
    public class DataContext : IdentityDbContext<AspUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
     
        }
        public DbSet<AspUser> users { get; set; }

    }
}
