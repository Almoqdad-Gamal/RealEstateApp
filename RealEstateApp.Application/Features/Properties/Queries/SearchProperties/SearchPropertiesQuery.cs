using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Application.Models;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Properties.Queries.SearchProperties
{
    public class SearchPropertiesQuery : IRequest<PaginatedResult<PropertyDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? City { get; set; }
        public string? Country { get; set; }

        public PropertyType? Type { get; set; }
        public ListingType? ListingType { get; set; }
        
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public decimal? MinArea { get; set; }
        public decimal? MaxArea { get; set; }

        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public int? MinBathrooms{ get; set; }

        public bool? HasPool { get; set; }
        public bool? HasGym { get; set; }
        public bool? HasGarden { get; set; }
        public bool? HasSecurity { get; set; }
        public bool? IsFurnished { get; set; }

        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
}