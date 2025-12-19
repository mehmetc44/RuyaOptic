using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RuyaOptik.DataAccess.Repositories.Configuration
{
    public static class AppConfiguration
    {
        public static string ConnectionString
        {
            get
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .Build();

                return configuration.GetConnectionString("SqlConnection")!;
            }
        }
    }
}
