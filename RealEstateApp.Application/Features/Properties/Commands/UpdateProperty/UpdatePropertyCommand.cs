using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommand : IRequest<PropertyDto>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public ListingType ListingType { get; set; }
        public PropertyStatus Status { get; set; }
        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int? ParkingSpaces { get; set; }
        public int? Floor { get; set; }
        public int? YearBuilt { get; set; }
        public bool HasGarden { get; set; }
        public bool HasPool { get; set; }
        public bool HasGym { get; set; }
        public bool HasSecurity { get; set; }
        public bool IsFurnished { get; set; }
    }
}