using Microsoft.AspNetCore.SignalR;
using RealEstateApp.API.Hubs;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;
        private readonly ICacheService _cache;

        public NotificationService(IHubContext<NotificationHub, INotificationClient> hubContext, IUnitOfWork unitOfWork, ILogger<NotificationService> logger, ICacheService cache)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
        }
        public async Task SendNotificationAsync(int userId, string title, string message, string type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _cache.RemoveAsync($"notifications_user_{userId}");

            _logger.LogInformation("Notification sent to user {UserId}: {Title}", userId, title);

            await _hubContext.Clients
                .Group($"user_{userId}")
                .ReceiveNotification(new
                {
                    notification.Id,
                    notification.Title,
                    notification.Message,
                    notification.Type,
                    notification.IsRead,
                    CreatedAt = notification.CreatedAt
                });
                
        }
    }
}