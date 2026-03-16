using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Properties.Queries.GetPropertiesByOwner
{
    public class GetPropertiesByOwnerQueryHandler : IRequestHandler<GetPropertiesByOwnerQuery, IEnumerable<PropertyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        public GetPropertiesByOwnerQueryHandler(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<IEnumerable<PropertyDto>> Handle(GetPropertiesByOwnerQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"properties_owner_{request.OwnerId}";

            var cached = await _cache.GetAsync<IEnumerable<PropertyDto>>(cacheKey);

            if(cached != null)
                return cached;

            
            var properties = await _unitOfWork.Properties.FindAync(p => p.OwnerId == request.OwnerId);

            var propertiesDto = properties.Select(p => new PropertyDto
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
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                OwnerId = p.OwnerId,
                CreatedAt = p.CreatedAt
            }).ToList();

            await _cache.SetAsync(cacheKey, propertiesDto, TimeSpan.FromMinutes(5));

            return propertiesDto;
        }
    }
}