using MediatR;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Properties.Commands.DeleteProperty
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(request.Id);
            if(property == null)
                throw new NotFoundException("Property", request.Id);

            //Soft delete
            _unitOfWork.Properties.Delete(property);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}