using MediatR;
using RealEstateApp.Application.DTOs.User;

namespace RealEstateApp.Application.Features.Users.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<LoginResponseDto>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}