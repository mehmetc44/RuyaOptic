using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using RuyaOptik.API.Extensions;
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
            builder.Services.AddAuthentication();

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
