using MediatR;
using RealEstateApp.Application.DTOs.Booking;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Booking.Commands.UpdateBookingStatus
{
    public class UpdateBookingStatusCommand : IRequest<BookingDto>
    {
        public int BookingId { get; set; }  
        public BookingStatus Status { get; set; }
    }
}