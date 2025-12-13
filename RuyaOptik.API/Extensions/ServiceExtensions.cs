using RuyaOptik.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RuyaOptik.Entity.Identity;
using RuyaOptik.DataAccess.Repositories.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                AppConfiguration.ConnectionString,
                b => b.MigrationsAssembly("RuyaOptik.DataAccess")
            )
            );
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<AspUser, AspRole>( o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
            .AddRoles<AspRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        }
        public static void ConfigureAutoMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(RuyaOptik.Business.Mapping.AutoMappingProfile));
        }
        public static void ConfigureDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<RuyaOptik.Business.Interfaces.ITokenService, RuyaOptik.Business.Services.TokenService>();
            
            services.AddScoped<RuyaOptik.Business.Interfaces.IAuthService, RuyaOptik.Business.Services.AuthService>();
        }

    }
}
