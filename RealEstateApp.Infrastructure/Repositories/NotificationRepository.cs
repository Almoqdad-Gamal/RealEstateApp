using Microsoft.EntityFrameworkCore;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Data;

namespace RealEstateApp.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _dbset.CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            return await _dbset
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(50)
                .ToListAsync();
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var unread = await _dbset.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();

            foreach (var notification in unread)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}