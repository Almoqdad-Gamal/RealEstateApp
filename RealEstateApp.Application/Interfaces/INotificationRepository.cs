using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync (int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkAllAsReadAsync(int userId);
    }
}