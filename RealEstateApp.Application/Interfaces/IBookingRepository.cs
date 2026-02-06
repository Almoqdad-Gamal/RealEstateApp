using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetUserBookingsAsync (int userId);
        Task<IEnumerable<Booking>> GetPropertyBookingsAsync (int propertyId);
        Task<bool> IsPropertyAvilableAsync (int propertyId, DateTime bookingDate, TimeSpan bookingTime);
        Task<IEnumerable<Booking>> GetBookingsByStatesAsync (BookingStatus status);
        
    }
}