using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Properties.Queries.SearchProperties
{
    public class SearchPropertiesQueryHandler : IRequestHandler<SearchPropertiesQuery, IEnumerable<PropertyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchPropertiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PropertyDto>> Handle(SearchPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = await _unitOfWork.Properties.SearchPropertiesAsync(
                city: request.City,
                type: request.Type,
                listingType: request.ListingType,
                minPrice: request.MinPrice,
                maxPrice: request.MaxPrice,
                minBedrooms: request.MinBedrooms
            );

            return properties.Select(p => new PropertyDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Type = p.Type,
                ListingType = p.ListingType,
                Status = p.Status,
                Price = p.Price,
                Area = p.Area,
                Address = p.Address,
                City = p.City,
                Country = p.Country,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                ParkingSpaces = p.ParkingSpaces,
                Floor = p.Floor,
                YearBuilt = p.YearBuilt,
                HasGarden = p.HasGarden,
                HasPool = p.HasPool,
                HasGym = p.HasGym,
                HasSecurity = p.HasSecurity,
                IsFurnished = p.IsFurnished,
                OwnerId = p.OwnerId,
                OwnerName = $"{p.Owner.FirstName} {p.Owner.LastName}",
                ImageUrls = p.Images.Select(i => i.ImageUrl).ToList(),
                CreatedAt = p.CreatedAt
            }).ToList();
        }
    }
}