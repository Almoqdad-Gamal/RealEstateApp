using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces
{
    public interface IJwtService 
    {
        string GenerateToken (User user);
    }
}