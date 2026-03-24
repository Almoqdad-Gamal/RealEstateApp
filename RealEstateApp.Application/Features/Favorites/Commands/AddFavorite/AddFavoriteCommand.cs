using MediatR;

namespace RealEstateApp.Application.Features.Favorites.Commands.AddFavorite
{
    public class AddFavoriteCommand : IRequest
    {
        public int UserId { get; set; }
        public int PropertyId { get; set; }
    }
}