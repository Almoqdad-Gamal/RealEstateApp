using MediatR;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Features.Properties.Commands.AddPropertyImage
{
    public class AddPropertyImageCommandHandler : IRequestHandler<AddPropertyImageCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _ImageService;

        public AddPropertyImageCommandHandler(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _ImageService = imageService;
        }
        public async Task<string> Handle(AddPropertyImageCommand request, CancellationToken cancellationToken)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(request.PropertyId);
            if (property == null)
                throw new NotFoundException("Property", request.PropertyId);


            if (property.OwnerId != request.RequestingUserId && request.RequestingRole != "Admin")
                throw new UnauthorizedException("You can only upload images to your own properties.");

            
            // Upload the photo on cloudinary
            var imageUrl = await _ImageService.UploadImageAsync(
                request.ImageStream,
                request.FileName
            );

            // Save the URL in the database
            var propertyImage = new PropertyImage
            {
                PropertyId = request.PropertyId,
                ImageUrl = imageUrl
            };

            await _unitOfWork.PropertyImages.AddAsync(propertyImage);
            await _unitOfWork.SaveChangesAsync();

            return imageUrl;
        }
    }
}