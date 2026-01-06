using Microsoft.AspNetCore.HttpOverrides;
using RuyaOptik.API.Extensions;
using Serilog;
using Serilog.Context;
using RuyaOptik.API.Hubs;

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

            // SignalR
            builder.Services.AddSignalR();

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

            // app.UseHttpsRedirection(); signalr test için kapalı

            app.UseStaticFiles();

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

            // Hub endpoint
            app.MapHub<OrdersHub>("/hubs/orders");

            app.Run();
        }
    }
}
