
using MediatR;
using RealEstateApp.Application.DTOs.Booking;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Booking.Queries.GetUserBookings
{
    public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, IEnumerable<BookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserBookingsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<BookingDto>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _unitOfWork.Bookings.GetUserBookingsAsync(request.UserId);

            return bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                BookingDate = b.BookingDate,
                BookingTime = b.BookingTime,
                Status = b.Status,
                Notes = b.Notes,
                PropertyId = b.PropertyId,
                PropertyTitle = b.Property.Title,
                PropertyCity = b.Property.City,
                ClientId = b.ClientId,
                ClientName = $"{b.Client.FirstName} {b.Client.LastName}",
                CreatedAt = b.CreatedAt
            }).ToList();
        }
    }
}