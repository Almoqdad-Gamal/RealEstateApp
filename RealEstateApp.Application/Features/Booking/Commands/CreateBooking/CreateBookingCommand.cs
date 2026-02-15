using MediatR;
using RealEstateApp.Application.DTOs.Booking;

namespace RealEstateApp.Application.Features.Booking.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<BookingDto>
    {
        public int PropertyId { get; set; }
        public int ClientId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public string? Notes { get; set; }
    }
}