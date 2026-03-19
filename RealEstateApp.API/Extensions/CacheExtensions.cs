using RealEstateApp.Application.Interfaces;
using RealEstateApp.Infrastructure.Services;
using StackExchange.Redis;

namespace RealEstateApp.API.Extensions
{
    public static class CacheExtensions
    {
        public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(
                    configuration.GetConnectionString("Redis")!
                )
            );

            services.AddScoped<ICacheService, RedisCacheService>();
            return services;
        }
    }
}