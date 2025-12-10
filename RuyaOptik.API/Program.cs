using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.Business.Mapping;
using RuyaOptik.Business.Services;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Concrete;
using RuyaOptik.DataAccess.Repositories.Interfaces;

namespace RuyaOptik.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DbContext -> Artık DataContext değil, RuyaOptikDbContext
            builder.Services.AddDbContext<RuyaOptikDbContext>(options =>
                options.UseSqlite(
                    builder.Configuration.GetConnectionString("SqlConnection"),
                    b => b.MigrationsAssembly("RuyaOptik.DataAccess")
                )
            );

            // Identity -> RuyaOptikDbContext üzerinden çalışacak
            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<RuyaOptikDbContext>();

            // Repositories
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            // Buraya ileride: IProductRepository, IOrderRepository vs. eklersin

            // Services
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();   // Identity için
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
