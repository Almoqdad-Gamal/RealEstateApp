using MediatR;
using RealEstateApp.Application.DTOs.Booking;

namespace RealEstateApp.Application.Features.Booking.Queries.GetUserBookings
{
    public class GetUserBookingsQuery : IRequest<IEnumerable<BookingDto>>
    {
        public int UserId { get; set; }
        public GetUserBookingsQuery(int userId)
        {
            UserId = userId;
        }
    }
}