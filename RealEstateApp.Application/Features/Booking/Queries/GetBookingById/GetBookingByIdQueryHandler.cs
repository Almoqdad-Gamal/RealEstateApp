using MediatR;
using RealEstateApp.Application.DTOs.Booking;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Booking.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetBookingByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(request.BookingId);

            if(booking == null)
                throw new NotFoundException("Booking", request.BookingId);

            return new BookingDto
            {
                Id = booking.Id,
                PropertyId = booking.PropertyId,
                ClientId = booking.ClientId,
                BookingDate = booking.BookingDate,
                BookingTime = booking.BookingTime,
                Status = booking.Status,
                Notes = booking.Notes,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}