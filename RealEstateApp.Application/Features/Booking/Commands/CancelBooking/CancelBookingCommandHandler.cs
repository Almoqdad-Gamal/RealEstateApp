using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Booking.Commands.CancelBooking
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CancelBookingCommandHandler> _logger;
        private readonly ICacheService _cache;
        public CancelBookingCommandHandler(IUnitOfWork unitOfWork,ILogger<CancelBookingCommandHandler> logger, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
            
        }
        public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(request.BookingId);

            if(booking == null)
                throw new NotFoundException("Booking", request.BookingId);

            // Only the client who booked the property can cancel the 
            if (booking.ClientId != request.RequestingUserId)
                throw new UnauthorizedException("You can only cancel your own bookings.");

            // A client can't cancel a confirmed booking.
            if (booking.Status == BookingStatus.Confirmed)
                throw new BadRequestException("Cannot cancel a confirmed booking.");

            booking.Status = BookingStatus.Canceled;

            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Booking {BookingId} cancelled by User {UserId}.", request.BookingId, request.RequestingUserId);

            await _cache.RemoveAsync($"bookings_user_{booking.ClientId}");

            await _cache.RemoveAsync("admin_stats");

        }
    }
}