using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Favorites.Commands.RemoveFavorite
{
    public class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly ILogger<RemoveFavoriteCommandHandler> _logger;
        public RemoveFavoriteCommandHandler(IUnitOfWork unitOfWork, ICacheService cache, ILogger<RemoveFavoriteCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }
        public async Task Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
        {
            var favorite = await _unitOfWork.Favorites.GetFavoriteAsync(request.UserId, request.PropertyId);

            if(favorite == null)
                throw new NotFoundException("Favorite", request.PropertyId);

            _unitOfWork.Favorites.Delete(favorite);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User {UserId} removed property {PropertyId} from favorites.", request.UserId, request.PropertyId);

            await _cache.RemoveAsync($"properties_user_{request.UserId}");
        }
    }
}