using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.Entity.Identity;
using RuyaOptik.DataAccess.Repositories.Configuration;
namespace RuyaOptik.DataAccess.Context
{
    public class DataContext : IdentityDbContext<AspUser,AspRole,string>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

        }
        public DbSet<AspUser> users { get; set; }

    }
}
