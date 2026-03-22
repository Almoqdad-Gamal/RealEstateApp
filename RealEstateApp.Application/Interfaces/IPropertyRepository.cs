using RealEstateApp.Application.Models;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Interfaces
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<IEnumerable<Property>> GetPropertiesByCityAsync(string city);
        Task<IEnumerable<Property>> GetPropertiesByTypeAsync(PropertyType type);
        Task<PaginatedResult<Property>> GetAvillablePropertiesAsync(PaginationParams pagination);
        Task<PaginatedResult<Property>> SearchPropertiesAsync(
            PaginationParams pagination,
            string? city = null,
            string? country = null,
            PropertyType? type = null,
            ListingType? listingType = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            decimal? minArea = null,
            decimal? maxArea = null,
            int? minBedrooms = null,
            int? maxBedrooms = null,
            int? minBathrooms = null,
            bool? hasPool = null,
            bool? hasGym = null,
            bool? hasGarden = null,
            bool? hasSecurity = null,
            bool? isFurnished = null,
            string? sortBy = null,
            string? sortOrder = null
        );

        Task<Property?> GetPropertyWithDetailsAsync(int id); 

        
    }
}