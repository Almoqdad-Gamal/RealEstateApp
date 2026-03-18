using MediatR;
using RealEstateApp.Application.DTOs.Booking;

namespace RealEstateApp.Application.Features.Booking.Queries.GetBookingById
{
    public class GetBookingByIdQuery : IRequest<BookingDto>
    {
        public int BookingId { get; set; }
        public int RequestingUserId { get; set; }
        public string RequestingRole { get; set; } = string.Empty;
        public GetBookingByIdQuery(int bookingId)
        {
            BookingId = bookingId;
        }
    }
}