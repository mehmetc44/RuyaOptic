using Microsoft.Extensions.Configuration;

namespace RuyaOptik.DataAccess.Repositories.Configuration
{
    public static class AppConfiguration
    {
        public static string ConnectionString
        {
            get
            {
                // 1. ADIM: Docker'daki (veya .env'den gelen) "SQL_CONNECTION_STRING" değişkenini oku.
                // BURASI BOŞ KALMAMALI, DEĞİŞKEN ADINI YAZIYORUZ:
                var envConn = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

                // Eğer Docker'dan dolu bir string geldiyse, hemen onu döndür.
                if (!string.IsNullOrWhiteSpace(envConn))
                    return envConn;

                // 2. ADIM: Docker'da değilsek (Local Visual Studio ile çalışıyorsak) appsettings.json'a bak.
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // Dosya yolunu garantiye al
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();

                // appsettings.json içindeki "ConnectionStrings:SqlConnection" kısmını oku
                return configuration.GetConnectionString("SqlConnection") ?? "";
            }
        }
    }
}
