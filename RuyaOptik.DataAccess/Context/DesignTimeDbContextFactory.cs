using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RuyaOptik.DataAccess.Repositories.Configuration;
namespace RuyaOptik.DataAccess.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RuyaOptikDbContext>
{
    public RuyaOptikDbContext CreateDbContext(string[] args)
    {
            DbContextOptionsBuilder<RuyaOptikDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer(AppConfiguration.ConnectionString);
            return new RuyaOptikDbContext(optionsBuilder.Options);
    }
}
   
}