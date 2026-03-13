using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Application.Models;

namespace RealEstateApp.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, PaginatedResult<PropertyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;

        public GetAllPropertiesQueryHandler(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }
        public async Task<PaginatedResult<PropertyDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            // Cache key changes with each page
            var cacheKey = $"properties_all_page{request.PageNumber}_size{request.PageSize}";
            
            // Look if there is any keys in the cache
            var cached = await _cache.GetAsync<PaginatedResult<PropertyDto>>(cacheKey);
            if(cached != null)
                return cached;

            var pagination = new PaginationParams
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
            var result = await _unitOfWork.Properties.GetAvillablePropertiesAsync(pagination);

            var dtos = result.Items.Select(p => new PropertyDto
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

            var paginatedResult = new PaginatedResult<PropertyDto>
            {
                Items = dtos,
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };

            // Save in the cache for 5 minutes
            await _cache.SetAsync(cacheKey, paginatedResult, TimeSpan.FromMinutes(5));

            return paginatedResult;
        }
    }
}