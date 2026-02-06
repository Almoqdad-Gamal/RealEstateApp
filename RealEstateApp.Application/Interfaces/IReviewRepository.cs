using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetPropertyReviewsAsync (int propertyId);
        Task<double> GetPropertyAverageRatingAsync (int propertyId);
        Task<bool> HasUserReviewedPropertyAsync (int userId, int propertyId);
    }
}