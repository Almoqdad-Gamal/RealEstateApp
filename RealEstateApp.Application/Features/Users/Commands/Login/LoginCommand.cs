using MediatR;
using RealEstateApp.Application.DTOs.User;

namespace RealEstateApp.Application.Features.Users.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}