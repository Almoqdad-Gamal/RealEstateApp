using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace RealEstateApp.API.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddHealthChecksServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(
                    configuration.GetConnectionString("DefaultConnection")!,
                    name: "database",
                    tags: new[] {"db", "sql"})
                .AddRedis(
                    configuration.GetConnectionString("Redis")!,
                    name: "redis",
                    tags: new[] {"cache", "redis"});

                return services;
        }

        public static IApplicationBuilder UseHealthChecksEndpoint(
            this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return app;
        }
    }
}