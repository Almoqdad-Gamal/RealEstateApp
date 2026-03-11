using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces
{
    public interface IJwtService 
    {
        string GenerateAccessToken (User user);
        string GenerateRefreshToken();

    }
}