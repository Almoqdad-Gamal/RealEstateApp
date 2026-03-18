using MediatR;
using RealEstateApp.Application.DTOs.User;

namespace RealEstateApp.Application.Features.Users.Commands.CreateAdmin
{
    public class CreateAdminCommand : IRequest<UserDto>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

    }
}