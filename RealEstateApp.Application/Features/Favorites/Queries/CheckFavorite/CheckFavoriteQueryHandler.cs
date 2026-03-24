using MediatR;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Favorites.Queries.CheckFavorite
{
    public class CheckFavoriteQueryHandler : IRequestHandler<CheckFavoriteQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CheckFavoriteQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(CheckFavoriteQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Favorites.isFavoriteAsync(request.UserId, request.PropertyId);
        }
    }
}