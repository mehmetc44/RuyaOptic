using Microsoft.Extensions.Configuration;

namespace RuyaOptik.DataAccess.Repositories.Configuration
{
    public static class AppConfiguration
    {
        public static string ConnectionString
        {
            get
            {
                // Docker / Prod (ENV)
                var envConn = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
                if (!string.IsNullOrWhiteSpace(envConn))
                    return envConn;

                // Local
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                return configuration.GetConnectionString("SqlConnection")!;
            }
        }
    }
}
