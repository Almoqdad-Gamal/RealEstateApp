using MediatR;
using RealEstateApp.Application.DTOs.Booking;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateBookingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // Check if the property is existing
            var property = await _unitOfWork.Properties.GetByIdAsync(request.PropertyId);
            if(property == null)
                throw new Exception("Property not found");

            // Check if the client is existing
            var client = await _unitOfWork.Users.GetByIdAsync(request.ClientId);
            if(client == null)
                throw new Exception("Client not found");

            // Check if the booking is available
            var isAvailable = await _unitOfWork.Bookings.IsPropertyAvilableAsync(
                request.PropertyId,
                request.BookingDate,
                request.BookingTime);

            if(!isAvailable)
                throw new Exception("This time is already booked");

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