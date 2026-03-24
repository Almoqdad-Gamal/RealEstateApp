using MediatR;

namespace RealEstateApp.Application.Features.Favorites.Commands.RemoveFavorite
{
    public class RemoveFavoriteCommand : IRequest
    {
        public int UserId { get; set; }
        public int PropertyId { get; set; }
    }
}