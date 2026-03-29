using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.DTOs.Admin;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Admin.Queries.GetAdminStats
{
    public class GetAdminStatsQueryHandler : IRequestHandler<GetAdminStatsQuery, AdminStatsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly ILogger<GetAdminStatsQueryHandler> _logger;

        public GetAdminStatsQueryHandler(IUnitOfWork unitOfWork, ICacheService cache, ILogger<GetAdminStatsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }
        public async Task<AdminStatsDto> Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "admin_stats";
            
            var cached = await _cache.GetAsync<AdminStatsDto>(cacheKey);
            if(cached != null)
                return cached;

            _logger.LogInformation("Fetching admin statistics.");

            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);

            var totalUsers = await _unitOfWork.Users.CountAsync();
            var totalProperties = await _unitOfWork.Properties.CountAsync();
            var totalBookings = await _unitOfWork.Bookings.CountAsync();
            var totalReviews = await _unitOfWork.Reviews.CountAsync();
            var totalFavorites = await _unitOfWork.Favorites.CountAsync();


            var newUsersThisMonth = await _unitOfWork.Users.CountAsync(u => u.CreatedAt >= startOfMonth);
            var newPropertiesThisMonth = await _unitOfWork.Properties.CountAsync(p => p.CreatedAt >= startOfMonth);
            var bookingsThisMonth = await _unitOfWork.Bookings.CountAsync(b => b.CreatedAt >= startOfMonth);

            var pendingBookings = await _unitOfWork.Bookings.CountAsync(b => b.Status == BookingStatus.Pending);
            var confirmedBookings = await _unitOfWork.Bookings.CountAsync(b => b.Status == BookingStatus.Confirmed);
            var completedBookings = await _unitOfWork.Bookings.CountAsync(b => b.Status == BookingStatus.Completed);
            var cancelledBookings = await _unitOfWork.Bookings.CountAsync(b => b.Status == BookingStatus.Canceled);

            var allProperties = await _unitOfWork.Properties.GetAllAsync();
            var propertiesList = allProperties.ToList();

            var propertiesByType = propertiesList.GroupBy(p => p.Type.ToString()).ToDictionary(g => g.Key, g => g.Count());
            var propertiesByCity = propertiesList.GroupBy(p => p.City).OrderByDescending(g => g.Count()).Take(10).ToDictionary(g => g.Key, g => g.Count());
            var propertiesByListingType = propertiesList.GroupBy(p => p.ListingType.ToString()).ToDictionary(g => g.Key, g => g.Count());


            // Top Rated Properties
            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            var reviewsList = reviews.ToList();

            var topRated = reviewsList
                .GroupBy(r => r.PropertyId)
                .Select(g => new
                {
                    PropertyId = g.Key,
                    AverageRating = g.Average(r => r.Rating),
                    ReviewCount = g.Count()
                })
                .OrderByDescending(x => x.AverageRating)
                .ThenByDescending(x => x.ReviewCount)
                .Take(5)
                .ToList();

            var topRatedDtos = new List<TopRatedPropertyDto>();
            foreach (var item in topRated)
            {
                var property = await _unitOfWork.Properties.GetByIdAsync(item.PropertyId);
                if(property != null)
                {
                    topRatedDtos.Add(new TopRatedPropertyDto
                    {
                        Id = property.Id,
                        Title = property.Title,
                        City = property.City,
                        Price = property.Price,
                        AverageRating = Math.Round(item.AverageRating, 1),
                        ReviewCount = item.ReviewCount
                    });
                }
            }

            var stats = new AdminStatsDto
            {
                TotalUsers = totalUsers,
                TotalProperties = totalProperties,
                TotalBookings = totalBookings,
                TotalReviews = totalReviews,
                TotalFavorites = totalFavorites,
                NewUsersThisMonth = newUsersThisMonth,
                NewPropertiesThisMonth = newPropertiesThisMonth,
                BookingsThisMonth = bookingsThisMonth,
                PendingBookings = pendingBookings,
                ConfirmedBookings = confirmedBookings,
                CompletedBookings = completedBookings,
                CancelledBookings = cancelledBookings,
                PropertiesByType = propertiesByType,
                PropertiesByCity = propertiesByCity,
                PropertiesByListingType = propertiesByListingType,
                TopRatedProperties = topRatedDtos
            };

            // I set the cache for just 2 minutes because stats changes a lot
            await _cache.SetAsync(cacheKey, stats, TimeSpan.FromMinutes(2));

            return stats;
        }
    }
}