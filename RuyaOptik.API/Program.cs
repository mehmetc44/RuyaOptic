using Microsoft.AspNetCore.HttpOverrides;
using RuyaOptik.API.Extensions;

namespace RuyaOptik.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ConfigureCors();
            // Add services to the container.
            builder.Services.ConfigureSQLContext(builder.Configuration);
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureAutoMappings();
            builder.Services.ConfigureDependencyInjections();
            builder.Services.ConfigureAuthentication(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureSwagger();
            //builder.Host.UseSerilog();
            /*Logger logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt")
            .CreateLogger();
            */

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
