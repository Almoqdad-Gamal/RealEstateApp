using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Notifications.Commands.MarkAllAsRead
{
    public class MarkAllAsReadCommandHandler : IRequestHandler<MarkAllAsReadCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly ILogger<MarkAllAsReadCommandHandler> _logger;
        public MarkAllAsReadCommandHandler(IUnitOfWork unitOfWork, ICacheService cache, ILogger<MarkAllAsReadCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }
        public async Task Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Notifications.MarkAllAsReadAsync(request.UserId);

            _logger.LogInformation("All notification marked as read for user {UserId}.", request.UserId);

            await _cache.RemoveAsync($"notfications_user_{request.UserId}");
        }
    }
}