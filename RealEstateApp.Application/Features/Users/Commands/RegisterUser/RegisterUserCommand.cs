using MediatR;
using RealEstateApp.Application.DTOs.User;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<UserDto>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}