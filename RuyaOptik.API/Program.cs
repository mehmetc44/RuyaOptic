using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.Business.Mapping;
using RuyaOptik.Business.Services;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Concrete;
using RuyaOptik.DataAccess.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<RuyaOptikDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("SqlConnection"),
        b => b.MigrationsAssembly("RuyaOptik.DataAccess")
    )
);

// Identity
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<RuyaOptikDbContext>();

// Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
