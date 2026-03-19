using RealEstateApp.Application.Interfaces;
using RealEstateApp.Infrastructure.Repositories;
using RealEstateApp.Infrastructure.Services;

namespace RealEstateApp.API.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IImageService, CloudinaryService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}