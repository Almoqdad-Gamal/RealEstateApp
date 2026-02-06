using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;
using RealEstateApp.Infrastructure.Data;

namespace RealEstateApp.Infrastructure.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {}

        public async Task<IEnumerable<Booking>> GetBookingsByStatesAsync(BookingStatus status)
        {
            return await _dbset
            .Where(b => b.Status == status)
            .Include(b => b.Property)
            .Include(b => b.Client)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetPropertyBookingsAsync(int propertyId)
        {
            return await _dbset
            .Where(b => b.PropertyId == propertyId)
            .Include(b => b.Client)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
        {
            return await _dbset
            .Where(b => b.ClientId == userId)
            .Include(b => b.Property)
                .ThenInclude(p => p.Images)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
        }

        public async Task<bool> IsPropertyAvilableAsync(int propertyId, DateTime bookingDate, TimeSpan bookingTime)
        {
            //Checking if there's already a booking at the same date and time
            var exsitingBooking = await _dbset
                .AnyAsync(b =>
                    b.PropertyId == propertyId &&
                    b.BookingDate.Date == bookingDate.Date &&
                    b.BookingTime == bookingTime &&
                    b.Status != BookingStatus.Canceled);

            return !exsitingBooking;
        }
    }
}