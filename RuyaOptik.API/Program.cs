using Microsoft.AspNetCore.HttpOverrides;
using RuyaOptik.API.Extensions;
using Serilog;
using Serilog.Context;

namespace RuyaOptik.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.ConfigureCors();
            builder.Services.ConfigureSQLContext(builder.Configuration);
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureAutoMappings();
            builder.Services.ConfigureDependencyInjections();
            builder.Services.ConfigureAuthentication(builder.Configuration);
            builder.Services.AddMemoryCache();
            builder.Services.AddResponseCaching();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureSwagger();
            builder.ConfigureSerilog();
            builder.Services.ConfigureHttpLogging();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseResponseCaching();

            app.UseAuthentication();
            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                var username = context.User?.Identity?.IsAuthenticated == true
                    ? context.User.Identity.Name
                    : "Anonymous";

                using (LogContext.PushProperty("UserName", username))
                {
                    await next();
                }
            });
            app.UseHttpLogging();
            app.UseSerilogRequestLogging();
            app.MapControllers();

            app.MapControllers();
            app.Run();
        }
    }
}

