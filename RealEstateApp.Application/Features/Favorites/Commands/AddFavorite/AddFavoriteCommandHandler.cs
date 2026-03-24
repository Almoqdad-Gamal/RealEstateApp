using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Features.Favorites.Commands.AddFavorite
{
    public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly ILogger<AddFavoriteCommandHandler> _logger;
        public AddFavoriteCommandHandler(IUnitOfWork unitOfWork, ICacheService cache, ILogger<AddFavoriteCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }
        public async Task Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(request.PropertyId);
            if(property == null)
                throw new NotFoundException("Property", request.PropertyId);

            var alreadyFavorited = await _unitOfWork.Favorites.isFavoriteAsync(request.UserId, request.PropertyId);
            if(alreadyFavorited)
                throw new ConflictException("Property is already in your favorites.");

            var favorite = new Favorite
            {
                UserId = request.UserId,
                PropertyId = request.PropertyId
            };

            await _unitOfWork.Favorites.AddAsync(favorite);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User {UserId} added property {PropertyId} to favorites.", request.UserId, request.PropertyId);

            await _cache.RemoveAsync($"favorites_user_{request.UserId}");
        }
    }
}