using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces
{
    public interface IFavoriteRepository : IGenericRepository<Favorite>
    {
        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(int userId);
        Task<bool> isFavoriteAsync(int userId, int propertyId);
        Task<Favorite?> GetFavoriteAsync(int userId, int propertyId);
    }
}