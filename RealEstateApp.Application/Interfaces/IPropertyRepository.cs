using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Interfaces
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<IEnumerable<Property>> GetPropertiesByCityAsync(string city);
        Task<IEnumerable<Property>> GetPropertiesByTypeAsync(PropertyType type);
        Task<IEnumerable<Property>> GetAvillablePropertiesAsync();
        Task<IEnumerable<Property>> SearchPropertiesAsync(
            string? city = null,
            PropertyType? type = null,
            ListingType? listingType= null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? minBedrooms = null
        );

        Task<Property?> GetPropertyWithDetailsAsync(int id); 

        
    }
}