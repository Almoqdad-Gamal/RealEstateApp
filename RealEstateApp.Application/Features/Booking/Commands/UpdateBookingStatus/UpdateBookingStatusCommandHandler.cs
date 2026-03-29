using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.DTOs.Booking;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Booking.Commands.UpdateBookingStatus
{
    public class UpdateBookingStatusCommandHandler : IRequestHandler<UpdateBookingStatusCommand, BookingDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateBookingStatusCommandHandler> _logger;
        private readonly INotificationService _notificationService;
        public UpdateBookingStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateBookingStatusCommandHandler> logger, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _notificationService = notificationService;
        }
        public async Task<BookingDto> Handle(UpdateBookingStatusCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(request.BookingId);

            if(booking == null)
                throw new NotFoundException("Booking", request.BookingId);

            booking.Status = request.Status;
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(
                userId: booking.ClientId,
                title: "Booking Status Updated",
                message: $"Your booking for '{booking.Property.Title}' is now {booking.Status}.",
                type: "BookingStatusUpdate"
            );
            
            _logger.LogInformation("Booking {BookingId} status updated to {Status}.", booking.Id, booking.Status);

            return new BookingDto
            {
                Id = booking.Id,
                BookingDate = booking.BookingDate,
                BookingTime = booking.BookingTime,
                Status = booking.Status,
                Notes = booking.Notes,
                PropertyId = booking.PropertyId,
                PropertyTitle = booking.Property.Title,
                PropertyCity = booking.Property.City,
                ClientId = booking.ClientId,
                ClientName = $"{booking.Client.FirstName} {booking.Client.LastName}",
                CreatedAt = booking.CreatedAt
            };
        }
    }
}