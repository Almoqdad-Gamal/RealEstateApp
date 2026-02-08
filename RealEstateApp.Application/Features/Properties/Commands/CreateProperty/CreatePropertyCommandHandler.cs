using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, PropertyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreatePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PropertyDto> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            //Verify owner exists
            var owner = await _unitOfWork.Users.GetByIdAsync(request.OwnerId);
            if(owner == null)
            {
                throw new Exception("Owner not found");
            }

            //Create property entity
            var property = new Property
            {
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                ListingType = request.ListingType,
                Status = PropertyStatus.Available,
                Price = request.Price,
                Area = request.Area,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Bedrooms = request.Bedrooms,
                Bathrooms = request.Bathrooms,
                ParkingSpaces = request.ParkingSpaces,
                Floor = request.Floor,
                YearBuilt = request.YearBuilt,
                HasGarden = request.HasGarden,
                HasPool = request.HasPool,
                HasGym = request.HasGym,
                HasSecurity = request.HasSecurity,
                IsFurnished = request.IsFurnished,
                OwnerId = request.OwnerId
            };

            //Add to repository and save 
            await _unitOfWork.Properties.AddAsync(property);
            await _unitOfWork.SaveChangesAsync();

            //Return DTO
            return new PropertyDto
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Type = property.Type,
                ListingType = property.ListingType,
                Status = property.Status,
                Price = property.Price,
                Area = property.Area,
                Address = property.Address,
                City = property.City,
                Country = property.Country,
                Latitude = property.Latitude,
                Longitude = property.Longitude,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                ParkingSpaces = property.ParkingSpaces,
                Floor = property.Floor,
                YearBuilt = property.YearBuilt,
                HasGarden = property.HasGarden,
                HasPool = property.HasPool,
                HasGym = property.HasGym,
                HasSecurity = property.HasSecurity,
                IsFurnished = property.IsFurnished,
                OwnerId = property.OwnerId,
                OwnerName = $"{owner.FirstName} {owner.LastName}",
                CreatedAt = property.CreatedAt
            };
        }
    }
}