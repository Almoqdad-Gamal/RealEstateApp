using MediatR;
using RealEstateApp.Application.DTOs.Booking;

namespace RealEstateApp.Application.Features.Booking.Queries.GetBookingById
{
    public class GetBookingByIdQuery : IRequest<BookingDto>
    {
        public int BookingId { get; set; }
        public GetBookingByIdQuery(int bookingId)
        {
            BookingId = bookingId;
        }
    }
}