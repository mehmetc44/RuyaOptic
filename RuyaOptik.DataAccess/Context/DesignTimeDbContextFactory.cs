using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RuyaOptik.DataAccess.Configurations;
namespace RuyaOptik.DataAccess.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RuyaOptikDbContext>
    {
        public RuyaOptikDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<RuyaOptikDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer(
    AppConfiguration.ConnectionString,
    b =>
    {
        b.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        );
    }
);

            return new RuyaOptikDbContext(optionsBuilder.Options);
        }
    }

}