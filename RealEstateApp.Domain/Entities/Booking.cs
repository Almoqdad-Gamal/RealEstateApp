using RealEstateApp.Domain.Common;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public string? Notes { get; set; }

        //Foreign Key
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public int ClientId { get; set; }
        public User Client { get; set; } = null!;
    }
}