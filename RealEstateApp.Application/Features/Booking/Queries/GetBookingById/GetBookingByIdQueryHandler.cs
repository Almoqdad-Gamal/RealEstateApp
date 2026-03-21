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
            var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(request.BookingId);

            if(booking == null)
                throw new NotFoundException("Booking", request.BookingId);

            var isClient = booking.ClientId == request.RequestingUserId;
            var isOwner = booking.Property.OwnerId == request.RequestingUserId;
            var isAdmin = request.RequestingRole == "Admin";

            if (!isClient && !isOwner && !isAdmin)
                throw new UnauthorizedException("You are not authorized to veiw this booking");

            return new BookingDto
            {
                Id = booking.Id,
                PropertyId = booking.PropertyId,
                PropertyTitle = booking.Property.Title,
                PropertyCity = booking.Property.City,
                ClientId = booking.ClientId,
                ClientName = $"{booking.Client.FirstName} {booking.Client.LastName}",
                BookingDate = booking.BookingDate,
                BookingTime = booking.BookingTime,
                Status = booking.Status,
                Notes = booking.Notes,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}