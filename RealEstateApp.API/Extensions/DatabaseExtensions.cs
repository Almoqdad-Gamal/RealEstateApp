using Microsoft.EntityFrameworkCore;
using RealEstateApp.Infrastructure.Data;

namespace RealEstateApp.API.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabase (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("RealEstateApp.Infrastructure")
                ));

            return services;
        }
    }
}