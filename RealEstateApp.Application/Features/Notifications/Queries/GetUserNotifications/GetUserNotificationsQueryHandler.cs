using MediatR;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Features.Notifications.Queries.GetUserNotifications
{
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, IEnumerable<Notification>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;

        public GetUserNotificationsQueryHandler(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }
        public async Task<IEnumerable<Notification>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"notifications_user_{request.UserId}";

            var cached = await _cache.GetAsync<IEnumerable<Notification>>(cacheKey);
            if(cached != null)
                return cached;

            var notifications = await _unitOfWork.Notifications.GetUserNotificationsAsync(request.UserId);

            await _cache.SetAsync(cacheKey, notifications, TimeSpan.FromMinutes(2));

            return notifications;
        }
    }
}