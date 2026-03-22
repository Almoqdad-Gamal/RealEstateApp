using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Application.Models;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Data;

namespace RealEstateApp.Infrastructure.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {}

        public async Task<PaginatedResult<Property>> GetAvillablePropertiesAsync(PaginationParams pagination)
        {
            var query = _dbset
                .Where(p => p.Status == PropertyStatus.Available)
                .Include(p => p.Images)
                .Include(p => p.Owner)
                .OrderByDescending(p => p.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResult<Property>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
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

        public async Task<PaginatedResult<Property>> SearchPropertiesAsync(
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
        )
        {
            var query = _dbset
            .Include(p => p.Images)
            .Include(p => p.Owner)
            .Where(p => p.Status == PropertyStatus.Available)
            .AsQueryable();

            //Apply filters
            if(!string.IsNullOrWhiteSpace(city))
                query = query.Where(p => p.City.ToLower().Contains(city.ToLower()));

            if(!string.IsNullOrWhiteSpace(country))
                query = query.Where(p => p.Country.ToLower().Contains(country.ToLower()));

            if(type.HasValue)
                query = query.Where(p => p.Type == type.Value);

            if(listingType.HasValue)
                query = query.Where(p => p.ListingType == listingType.Value);

            if(minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if(maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (minArea.HasValue)
                query = query.Where(p => p.Area >= minArea.Value);

            if (maxArea.HasValue)
                query = query.Where(p => p.Area <= maxArea.Value);

            if(minBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms >= minBedrooms.Value);

            if(maxBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms <= maxBedrooms.Value);

            if(minBathrooms.HasValue)
                query = query.Where(p => p.Bathrooms >= minBathrooms.Value);

            if (hasPool.HasValue)
                query = query.Where(p => p.HasPool == hasPool.Value);

            if (hasGym.HasValue)
                query = query.Where(p => p.HasGym == hasGym.Value);

            if (hasGarden.HasValue)
                query = query.Where(p => p.HasGarden == hasGarden.Value);
    
            if (hasSecurity.HasValue)
                query = query.Where(p => p.HasSecurity == hasSecurity.Value);

            if (isFurnished.HasValue)
                query = query.Where(p => p.IsFurnished == isFurnished.Value);

            // Sorting
            var isDescending = sortOrder?.ToLower() == "desc";

            query = sortBy?.ToLower() switch
            {
                "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "area"  => isDescending ? query.OrderByDescending(p => p.Area)  : query.OrderBy(p => p.Area),
                "bedrooms"=> isDescending ? query.OrderByDescending(p => p.Bedrooms) : query.OrderBy(p => p.Bedrooms),
                    _     => query.OrderByDescending(p => p.CreatedAt) // Default
            };


            // Pagination
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginatedResult<Property>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}