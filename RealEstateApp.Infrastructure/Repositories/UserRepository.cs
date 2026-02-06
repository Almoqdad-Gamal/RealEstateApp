using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Data;

namespace RealEstateApp.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {}

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbset.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbset.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserWithPropertiesAsync(int userId)
        {
            return await _dbset
            .Include(u => u.Properties)
                .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}