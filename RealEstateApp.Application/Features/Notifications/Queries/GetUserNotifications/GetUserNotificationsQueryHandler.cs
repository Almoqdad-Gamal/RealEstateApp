using MediatR;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Features.Notifications.Queries.GetUserNotifications
{
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, IEnumerable<Notification>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserNotificationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Notification>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Notifications.GetUserNotificationsAsync(request.UserId);
        }
    }
}