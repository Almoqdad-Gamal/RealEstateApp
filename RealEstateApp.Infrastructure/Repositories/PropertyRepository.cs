using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Data;

namespace RealEstateApp.Infrastructure.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {}

        public async Task<IEnumerable<Property>> GetAvillablePropertiesAsync()
        {
            return await _dbset
            .Where(p => p.Status == PropertyStatus.Available)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByCityAsync(string city)
        {
            return await _dbset
            .Where(p => p.City.ToLower() == city.ToLower())
            .Include(p => p.Images)
            .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByTypeAsync(PropertyType type)
        {
            return await _dbset
            .Where(p => p.Type == type)
            .Include(p => p.Images)
            .ToListAsync();
        }

        public async Task<Property?> GetPropertyWithDetailsAsync(int id)
        {
            return await _dbset
            .Include(p => p.Images)
            .Include(p => p.Reviews)
                .ThenInclude(p => p.User)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Property>> SearchPropertiesAsync(string? city = null, PropertyType? type = null, ListingType? listingType = null, decimal? minPrice = null, decimal? maxPrice = null, int? minBedrooms = null)
        {
            var query = _dbset.AsQueryable();

            //Apply filters
            if(!string.IsNullOrWhiteSpace(city))
                query = query.Where(p => p.City.ToLower().Contains(city.ToLower()));

            if(type.HasValue)
                query = query.Where(p => p.Type == type.Value);

            if(listingType.HasValue)
                query = query.Where(p => p.ListingType == listingType.Value);

            if(minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if(maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if(minBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms >= minBedrooms.Value);

            //Only availabele properties
            query = query.Where(p => p.Status == PropertyStatus.Available);

            return await query
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        }
    }
}