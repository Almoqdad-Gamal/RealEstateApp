using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Data;

namespace RealEstateApp.Infrastructure.Repositories
{
    public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
    {

        public FavoriteRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Favorite?> GetFavoriteAsync(int userId, int propertyId)
        {
            return await _dbset
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);
        }

        public async Task<IEnumerable<Favorite>> GetUserFavoritesAsync(int userId)
        {
            return await _dbset
                .Where(f => f.UserId == userId)
                .Include(f => f.Property)
                    .ThenInclude(p => p.Images)
                .Include(f => f.Property)
                    .ThenInclude(p => p.Owner)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> isFavoriteAsync(int userId, int propertyId)
        {
            return await _dbset
                .AnyAsync(f => f.UserId == userId && f.PropertyId == propertyId);
        }

    }
}