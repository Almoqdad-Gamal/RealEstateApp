using MediatR;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Properties.Commands.DeleteProperty
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;

        public DeletePropertyCommandHandler(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }
        public async Task Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(request.Id);
            if(property == null)
                throw new NotFoundException("Property", request.Id);

            if (property.OwnerId != request.RequestingUserId && request.RequestingRole != "Admin")
                throw new UnauthorizedException("You can only delete your own properties.");

            //Soft delete
            _unitOfWork.Properties.Delete(property);
            await _unitOfWork.SaveChangesAsync();
            await _cache.RemoveByPrefixAsync("properties_all");
            await _cache.RemoveAsync($"property_{request.Id}");
        }
    }
}