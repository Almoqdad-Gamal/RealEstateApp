using MediatR;

namespace RealEstateApp.Application.Features.Favorites.Queries.CheckFavorite
{
    public class CheckFavoriteQuery : IRequest<bool>
    {
        public int UserId { get; set; }
        public int PropertyId { get; set; }

        public CheckFavoriteQuery(int userId, int propertyId)
        {
            UserId = userId;
            PropertyId = propertyId;
        }
    }
}