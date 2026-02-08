using MediatR;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Properties.Commands.DeleteProperty
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(request.Id);
            if(property == null)
            {
                return false;
            }

            //Soft delete
            _unitOfWork.Properties.Delete(property);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}