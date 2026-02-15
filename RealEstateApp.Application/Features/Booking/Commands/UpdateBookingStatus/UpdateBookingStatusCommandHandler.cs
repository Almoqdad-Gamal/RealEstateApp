using MediatR;
using RealEstateApp.Application.DTOs.Booking;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Booking.Commands.UpdateBookingStatus
{
    public class UpdateBookingStatusCommandHandler : IRequestHandler<UpdateBookingStatusCommand, BookingDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateBookingStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BookingDto> Handle(UpdateBookingStatusCommand request, CancellationToken cancellationToken)
        {
            var bookings = await _unitOfWork.Bookings.GetUserBookingsAsync(request.BookingId);
            var booking = bookings.FirstOrDefault(b => b.Id == request.BookingId);

            if(booking == null)
                throw new Exception("Booking is found");

            booking.Status = request.Status;
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync();

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