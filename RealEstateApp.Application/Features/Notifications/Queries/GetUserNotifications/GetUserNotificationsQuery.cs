using MediatR;
using RealEstateApp.Domain.Entities;


namespace RealEstateApp.Application.Features.Notifications.Queries.GetUserNotifications
{
    public class GetUserNotificationsQuery : IRequest<IEnumerable<Notification>>
    {
        public int UserId { get; set; }

        public GetUserNotificationsQuery(int userId)
        {
            UserId = userId;
        }
    }
}