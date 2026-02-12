using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Application.Features.Properties.Queries.GetAllProperties.GetPropertyById;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Properties.Queries.GetPropertyById
{
    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPropertyByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PropertyDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(request.PropertyId);

            if (property == null)
                throw new Exception("Property not found");

            // Calculate average rating
            var avgRating = await _unitOfWork.Reviews.GetPropertyAverageRatingAsync(property.Id);

            return new PropertyDto
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
        }
    }
}