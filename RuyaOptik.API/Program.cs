using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.API.Middlewares;          // 🔴 EKLENDİ
using RuyaOptik.Business.Interfaces;
using RuyaOptik.Business.Mapping;
using RuyaOptik.Business.Services;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Concrete;
using RuyaOptik.DataAccess.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// DATABASE
builder.Services.AddDbContext<RuyaOptikDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("SqlConnection"),
        b => b.MigrationsAssembly("RuyaOptik.DataAccess")
    )
);

// IDENTITY
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<RuyaOptikDbContext>();

// REPOSITORIES
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

// SERVICES
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();

// AUTOMAPPER
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// CONTROLLERS & SWAGGER
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// GLOBAL EXCEPTION MIDDLEWARE
app.UseMiddleware<ExceptionMiddleware>();

// SWAGGER
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// PIPELINE
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
