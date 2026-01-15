using RuyaOptik.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RuyaOptik.Entity.Identity;
using RuyaOptik.DataAccess.Repositories.Configuration;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.Business.Services;
using RuyaOptik.DataAccess.Repositories.Concrete;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using Serilog;
using Microsoft.AspNetCore.HttpLogging;

namespace RuyaOptik.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
            builder
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
        );
    });




        public static void ConfigureSQLContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RuyaOptikDbContext>(options =>
            options.UseSqlServer(
                AppConfiguration.ConnectionString,
                b => b.MigrationsAssembly("RuyaOptik.DataAccess")
            )
            );
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<AspUser, AspRole>(o =>
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
            services.AddAutoMapper(typeof(RuyaOptik.Business.Mapping.AutoMapperProfile));
        }

        public static void ConfigureDependencyInjections(this IServiceCollection services)
        {
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
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IMailService, MailService>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "")
                    ),
                    ClockSkew = TimeSpan.Zero, // token süresi bitince tolerans olmasın

                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };

                // SIGNALR: token querystring’ten de okunabilsin
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs/orders"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RuyaOptik API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT token girin."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
            });
        }

        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            var columnOptions = new ColumnOptions();

            columnOptions.AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn
                {
                    ColumnName = "UserName",
                    DataType = SqlDbType.NVarChar,
                    DataLength = 50,
                    AllowNull = true
                }
            };

            columnOptions.Store.Clear();
            columnOptions.Store.Add(StandardColumn.LogEvent);
            columnOptions.Store.Add(StandardColumn.Exception);
            columnOptions.Store.Add(StandardColumn.MessageTemplate);
            columnOptions.Store.Add(StandardColumn.Message);
            columnOptions.Store.Add(StandardColumn.Level);
            columnOptions.Store.Add(StandardColumn.TimeStamp);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "logs/log.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7
                )
                /*.WriteTo.MSSqlServer(
                    connectionString: AppConfiguration.ConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true
                    },
                    columnOptions: columnOptions
                )*/
                .CreateLogger();

            builder.Host.UseSerilog(Log.Logger);
        }

        public static void ConfigureHttpLogging(this IServiceCollection services)
        {
            services.AddHttpLogging(options =>
            {
                options.LoggingFields =
                    HttpLoggingFields.RequestMethod |
                    HttpLoggingFields.RequestPath |
                    HttpLoggingFields.RequestHeaders |
                    HttpLoggingFields.ResponseStatusCode |
                    HttpLoggingFields.ResponseHeaders;

                options.RequestBodyLogLimit = 4096;
                options.ResponseBodyLogLimit = 4096;

                options.RequestHeaders.Remove("Authorization");
                options.RequestHeaders.Remove("Cookie");
                options.RequestHeaders.Remove("Set-Cookie");

                options.ResponseHeaders.Remove("Set-Cookie");

                options.MediaTypeOptions.AddText("application/json");
                options.MediaTypeOptions.AddText("application/problem+json");
            });
        }
    }
}
