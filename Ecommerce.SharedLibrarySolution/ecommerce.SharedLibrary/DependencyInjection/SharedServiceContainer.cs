using ecommerce.SharedLibrary.middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedService<TContext>(this IServiceCollection services,  IConfiguration config, string fileName) where TContext : DbContext
        {
            // Add Generic Database context
            services.AddDbContext<TContext>(options =>
            options.UseMySql(config.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(config.GetConnectionString("DefaultConnection")),
            mysqlOptions => { mysqlOptions.EnableRetryOnFailure(); }));


            // Configure serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy:MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();


            // Add JWT authentication scheme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, config);

            return services;
        }


        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        { 
            // Use global exception
            app.UseMiddleware<GlobalException>();


            // Register middleware to block outsiders API calls
            app.UseMiddleware<ListenToOnlyApiGateway>();

            return app; 
        }
    }
}
