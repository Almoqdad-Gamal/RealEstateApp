namespace RealEstateApp.Application.DTOs.Booking
{
    public class CreateBookingDto
    {
        public int PropertyId { get; set; }
        public int ClientId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public string? Notes { get; set; }
    }
}