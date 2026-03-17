using MediatR;

namespace RealEstateApp.Application.Features.Booking.Commands.CancelBooking
{
    public class CancelBookingCommand : IRequest
    {
        public int BookingId { get; set; }
        public int RequestingUserId { get; set; }

        public CancelBookingCommand(int bookingId)
        {
            BookingId = bookingId;
        }
    }
}