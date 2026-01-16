using Microsoft.Extensions.Configuration;

namespace RuyaOptik.DataAccess.Configurations
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
        public static string jwtKey =>
        Environment.GetEnvironmentVariable("JWT_KEY") 
        ?? throw new Exception("JWT_KEY missing!");

    public static string issuer =>
        Environment.GetEnvironmentVariable("JWT_ISSUER") 
        ?? throw new Exception("JWT_ISSUER missing!");

    public static string audience =>
        Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
        ?? throw new Exception("JWT_AUDIENCE missing!");

    public static string MailHost =>
        Environment.GetEnvironmentVariable("MAIL_HOST") 
        ?? throw new Exception("MAIL_HOST missing!");

    public static int MailPort =>
        int.TryParse(Environment.GetEnvironmentVariable("MAIL_PORT"), out var port) ? port : 587;

    public static string MailUsername =>
        Environment.GetEnvironmentVariable("MAIL_USER") 
        ?? throw new Exception("MAIL_USER missing!");

    public static string MailPassword =>
        Environment.GetEnvironmentVariable("MAIL_PASSWORD") 
        ?? throw new Exception("MAIL_PASSWORD missing!");

    public static string ClientUrl =>
        Environment.GetEnvironmentVariable("CLIENT_URL") ?? "http://localhost:8080";
    }
}
