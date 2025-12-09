using RuyaOptik.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
namespace RuyaOptik.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options =>
        {
        });
        public static void ConfigureSqliteContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("SqlConnection"),
                b => b.MigrationsAssembly("RuyaOptik.DataAccess")
            )
            );
        }
    }
}
