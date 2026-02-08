using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Properties.Queries.SearchProperties
{
    public class SearchPropertiesQuery : IRequest<IEnumerable<PropertyDto>>
    {
        public string? City { get; set; }
        public PropertyType? Type { get; set; }
        public ListingType? ListingType { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinBedrooms { get; set; }
    }
}