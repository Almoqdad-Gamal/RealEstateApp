using MediatR;
using RealEstateApp.Application.DTOs.Favorite;

namespace RealEstateApp.Application.Features.Favorites.Queries.GetUserFavorites
{
    public class GetUserFavoritesQuery : IRequest<IEnumerable<FavoriteDto>>
    {
        public int UserId { get; set; }
        public GetUserFavoritesQuery(int userId)
        {
            UserId = userId;
        }
    }
}