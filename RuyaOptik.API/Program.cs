using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using RuyaOptik.API.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace RuyaOptik.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ConfigureCors();
            builder.Services.ConfigureIISIntegration();
            // Add services to the container.
            builder.Services.ConfigureSqliteContext(builder.Configuration);
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureAutoMappings();
            builder.Services.ConfigureDependencyInjections();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Admin", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Oluşacak tokenin hangi sunucu tarafından oluşturulacağını belirler (bizim sunucumuz)
                    ValidateAudience = true,//Oluşturulacak tokenin hangi kullanıcının hangi sitelerde geçerli olacağını belirler
                    ValidateLifetime = true,//tokenin süresinin dolup dolmadığını kontrol eder
                    ValidateIssuerSigningKey = true,//yazdığımız securuty key'in doğruluğunu kontrol eder

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
                
                
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
                app.UseHsts();
            app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}
