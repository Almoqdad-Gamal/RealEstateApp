using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, PropertyDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PropertyDto> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            //Geting existing property with owner details
            var property = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(request.Id);
            if(property == null)
            {
                throw new Exception("Property not found");
            }

            //Update property fields
            property.Title = request.Title;
            property.Description = request.Description;
            property.Type = request.Type;
            property.ListingType = request.ListingType;
            property.Status = request.Status;
            property.Price = request.Price;
            property.Area = request.Area;
            property.Address = request.Address;
            property.City = request.City;
            property.Country = request.Country;
            property.Latitude = request.Latitude;
            property.Longitude = request.Longitude;
            property.Bedrooms = request.Bedrooms;
            property.Bathrooms = request.Bathrooms;
            property.ParkingSpaces = request.ParkingSpaces;
            property.Floor = request.Floor;
            property.YearBuilt = request.YearBuilt;
            property.HasGarden = request.HasGarden;
            property.HasPool = request.HasPool;
            property.HasGym = request.HasGym;
            property.HasSecurity = request.HasSecurity;
            property.IsFurnished = request.IsFurnished;

            //Update in repository
            _unitOfWork.Properties.Update(property);
            await _unitOfWork.SaveChangesAsync();

            //Calculate average rating
            var avgRating = await _unitOfWork.Reviews.GetPropertyAverageRatingAsync(property.Id);

            //Return updated DTO
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
                OwnerName = $"{property.Owner.FirstName} {property.Owner.LastName}",
                ImageUrls = property.Images.Select(i => i.ImageUrl).ToList(),
                AverageRating = avgRating,
                ReviewCount = property.Reviews.Count,
                CreatedAt = property.CreatedAt
            };

        }
    }
}