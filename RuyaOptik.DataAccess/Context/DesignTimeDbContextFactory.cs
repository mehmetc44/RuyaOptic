using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RuyaOptik.DataAccess.Repositories.Configuration;
namespace RuyaOptik.DataAccess.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RuyaOptikDbContext>
{
    public RuyaOptikDbContext CreateDbContext(string[] args)
    {
            DbContextOptionsBuilder<RuyaOptikDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlite(AppConfiguration.ConnectionString);
            return new RuyaOptikDbContext(optionsBuilder.Options);
    }
}


    
}