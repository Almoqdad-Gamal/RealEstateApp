using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.DTOs.Booking;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBookingCommandHandler> _logger;
        private readonly ICacheService _cache;
        public CreateBookingCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateBookingCommandHandler> logger, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }
        public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // Check if the property is existing
            var property = await _unitOfWork.Properties.GetByIdAsync(request.PropertyId);
            if(property == null)
                throw new NotFoundException("Property", request.PropertyId);

            // Check if the client is existing
            var client = await _unitOfWork.Users.GetByIdAsync(request.ClientId);
            if(client == null)
                throw new NotFoundException("Client", request.ClientId);

            // Check if the booking is available
            var isAvailable = await _unitOfWork.Bookings.IsPropertyAvilableAsync(
                request.PropertyId,
                request.BookingDate,
                request.BookingTime);

            if(!isAvailable)
                throw new ConflictException("This time slot is already booked for this property.");

            // Create booking
            var booking = new Domain.Entities.Booking
            {
                PropertyId = request.PropertyId,
                ClientId = request.ClientId,
                BookingDate = request.BookingDate,
                BookingTime = request.BookingTime,
                Notes = request.Notes,
                Status = BookingStatus.Pending
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Booking created with ID: {BookingId} for Property: {PropertyId} by Client: {ClientId}",
                booking.Id, booking.PropertyId, booking.ClientId);

            await _cache.RemoveAsync($"booking_user_{booking.ClientId}");

            return new BookingDto
            {
                Id = booking.Id,
                BookingDate = booking.BookingDate,
                BookingTime = booking.BookingTime,
                Status = booking.Status,
                Notes = booking.Notes,
                PropertyId = property.Id,
                PropertyTitle = property.Title,
                PropertyCity = property.City,
                ClientId = client.Id,
                ClientName = $"{client.FirstName} {client.LastName}",
                CreatedAt = booking.CreatedAt

            };
        }
    }
}