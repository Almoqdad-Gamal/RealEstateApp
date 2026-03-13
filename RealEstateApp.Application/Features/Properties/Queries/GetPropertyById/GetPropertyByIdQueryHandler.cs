using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Features.Properties.Queries.GetAllProperties.GetPropertyById;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Properties.Queries.GetPropertyById
{
    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;

        public GetPropertyByIdQueryHandler(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<PropertyDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"property_{request.PropertyId}";

            var cached = await _cache.GetAsync<PropertyDto>(cacheKey);
            if(cached != null)
                return cached;


            var property = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(request.PropertyId);

            if (property == null)
                throw new NotFoundException("Property", request.PropertyId);

            // Calculate average rating
            var avgRating = await _unitOfWork.Reviews.GetPropertyAverageRatingAsync(property.Id);

            var dto = new PropertyDto
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Type = property.Type,
                ListingType = property.ListingType,
                Status = property.Status,
                Price = property.Price,
                Area = property.Area,
                Address = property.Address,
                City = property.City,
                Country = property.Country,
                Latitude = property.Latitude,
                Longitude = property.Longitude,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                ParkingSpaces = property.ParkingSpaces,
                Floor = property.Floor,
                YearBuilt = property.YearBuilt,
                HasGarden = property.HasGarden,
                HasPool = property.HasPool,
                HasGym = property.HasGym,
                HasSecurity = property.HasSecurity,
                IsFurnished = property.IsFurnished,
                OwnerId = property.OwnerId,
                OwnerName = $"{property.Owner.FirstName} {property.Owner.LastName}",
                ImageUrls = property.Images.Select(i => i.ImageUrl).ToList(),
                AverageRating = avgRating,
                ReviewCount = property.Reviews.Count,
                CreatedAt = property.CreatedAt
            };

            await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

            return dto;
        }
    }
}