using MediatR;

namespace RealEstateApp.Application.Features.Notifications.Queries.GetUnreadCount
{
    public class GetUnreadCountQuery : IRequest<int>
    {
        public int UserId { get; set; }

        public GetUnreadCountQuery(int userId)
        {
            UserId = userId;
        }
    }
}