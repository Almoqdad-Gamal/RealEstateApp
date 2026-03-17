
using MediatR;
using RealEstateApp.Application.DTOs.Booking;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Booking.Queries.GetUserBookings
{
    public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, IEnumerable<BookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        public GetUserBookingsQueryHandler(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }
        public async Task<IEnumerable<BookingDto>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"bookings_user_{request.UserId}";
            var cached = await _cache.GetAsync<IEnumerable<BookingDto>>(cacheKey);

            if(cached != null)
                return cached;

            var bookings = await _unitOfWork.Bookings.GetUserBookingsAsync(request.UserId);

            var bookingDtos = bookings.Select(b => new BookingDto
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

            await _cache.SetAsync(cacheKey, bookingDtos, TimeSpan.FromMinutes(3));

            return bookingDtos;
        }
    }
}