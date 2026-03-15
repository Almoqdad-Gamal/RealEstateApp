using MediatR;
using RealEstateApp.Application.DTOs.User;

namespace RealEstateApp.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<UserDto>
    {
        // It's fill from the token not the body
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}