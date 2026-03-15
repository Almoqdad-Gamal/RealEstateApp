using MediatR;
using RealEstateApp.Application.DTOs.User;

namespace RealEstateApp.Application.Features.Users.Queries.GetProfile
{
    public class GetProfileQuery : IRequest<UserDto>
    {
        public int UserId { get; set; }

        public GetProfileQuery(int userId)
        {
            UserId = userId;
        }
    }
}