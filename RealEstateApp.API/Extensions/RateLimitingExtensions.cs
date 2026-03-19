using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace RealEstateApp.API.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddRateLimiting (this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("auth", config =>
                {
                    config.PermitLimit = 5;
                    config.Window = TimeSpan.FromMinutes(1);
                    config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    config.QueueLimit = 0;
                });

                options.AddFixedWindowLimiter("api", config =>
                {
                    config.PermitLimit = 100;
                    config.Window = TimeSpan.FromMinutes(1);
                    config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    config.QueueLimit = 0;
                });

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        statusCode = 429,
                        message = "Too many requests. Please try again later."
                    }, cancellationToken);
                };
            });

            return services;
        }
    }
}