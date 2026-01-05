using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.Entity.Identity;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.Business.Services;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DataAccess.Repositories.Concrete;

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
            services.Configure<IISOptions>(options => { });

        public static void ConfigureSqliteContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<RuyaOptikDbContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("Sqlite"),
                    b => b.MigrationsAssembly("RuyaOptik.DataAccess")
                ));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<AspUser, AspRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
            .AddRoles<AspRole>()
            .AddEntityFrameworkStores<RuyaOptikDbContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureAutoMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(RuyaOptik.Business.Mapping.AutoMapperProfile)
            );
        }

        public static void ConfigureDependencyInjections(
            this IServiceCollection services)
        {
            // Cache Versioning Service
            services.AddSingleton<CacheVersionService>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICartService, CartService>();

            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMailService, MailService>();

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        }
    }
}
