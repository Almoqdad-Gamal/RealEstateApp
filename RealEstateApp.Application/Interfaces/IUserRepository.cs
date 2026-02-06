using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> GetUserWithPropertiesAsync(int userId);


    }
}