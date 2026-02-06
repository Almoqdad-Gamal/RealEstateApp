using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Data;

namespace RealEstateApp.Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<double> GetPropertyAverageRatingAsync(int propertyId)
        {
            var reviews = await _dbset
            .Where(r => r.PropertyId == propertyId)
            .ToListAsync();

            if(!reviews.Any())
                return 0;
            
            return reviews.Average(r => r.Rating);
        }

        public async Task<IEnumerable<Review>> GetPropertyReviewsAsync(int propertyId)
        {
            return await _dbset
            .Where(r => r.PropertyId == propertyId)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
        }

        public async Task<bool> HasUserReviewedPropertyAsync(int userId, int propertyId)
        {
            return await _dbset
            .AnyAsync(r => r.UserId == userId && r.PropertyId == propertyId);
        }
    }
}