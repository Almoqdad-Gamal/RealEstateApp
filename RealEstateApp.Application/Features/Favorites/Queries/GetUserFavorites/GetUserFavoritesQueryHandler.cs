using MediatR;
using RealEstateApp.Application.DTOs.Favorite;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Favorites.Queries.GetUserFavorites
{
    public class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, IEnumerable<FavoriteDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        public GetUserFavoritesQueryHandler(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<IEnumerable<FavoriteDto>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"favorites_user_{request.UserId}";
            var cached = await _cache.GetAsync<IEnumerable<FavoriteDto>>(cacheKey);
            if(cached != null)
                return cached;

            var favorites = await _unitOfWork.Favorites.GetUserFavoritesAsync(request.UserId);

            var favoriteDtos = favorites.Select(f => new FavoriteDto
            {
                Id = f.Id,
                PropertyId = f.PropertyId,
                PropertyTitle = f.Property.Title,
                PropertyCity = f.Property.City,
                PropertyCountry = f.Property.Country,
                PropertyPrice = f.Property.Price,
                PrimaryImageUrl = f.Property.Images
                    .FirstOrDefault(i => i.IsPrimary)?.ImageUrl
                    ?? f.Property.Images.FirstOrDefault()?.ImageUrl,
                Bedrooms = f.Property.Bedrooms,
                Bathrooms = f.Property.Bathrooms,
                Area = f.Property.Area,
                AddedAt = f.CreatedAt
            }).ToList();

            await _cache.SetAsync(cacheKey, favoriteDtos, TimeSpan.FromMinutes(5));

            return favoriteDtos;
        }
    }
}