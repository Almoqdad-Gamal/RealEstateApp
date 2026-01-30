using RealEstateApp.Domain.Common;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Entities
{
    public class Property : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public ListingType ListingType { get; set; }
        public PropertyStatus Status { get; set; } = PropertyStatus.Available;

        //Price & Area
        public decimal Price { get; set; }
        public decimal Area { get; set; }

        //Location
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        //Property Details
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int? ParkingSpaces { get; set; }
        public int? Floor { get; set; }
        public int? YearBuilt { get; set; }

        //Features
        public bool HasGarden { get; set; }
        public bool HasPool { get; set; }
        public bool HasGym { get; set; }
        public bool HasSecurity { get; set; }
        public bool IsFurnished { get; set; }

        //Owner Info
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        //Navigation Properties
        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}