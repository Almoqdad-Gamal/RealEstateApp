using MediatR;
using RealEstateApp.Application.DTOs.Property;

namespace RealEstateApp.Application.Features.Properties.Queries.GetAllProperties.GetPropertyById
{
    public class GetPropertyByIdQuery : IRequest<PropertyDto>
    {
        public int PropertyId { get; set; }

        public GetPropertyByIdQuery(int propertyId)
        {
            PropertyId = propertyId;
        }
    }
}