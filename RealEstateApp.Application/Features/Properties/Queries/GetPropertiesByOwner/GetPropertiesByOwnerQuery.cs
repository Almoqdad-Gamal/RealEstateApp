using MediatR;
using RealEstateApp.Application.DTOs.Property;

namespace RealEstateApp.Application.Features.Properties.Queries.GetPropertiesByOwner
{
    public class GetPropertiesByOwnerQuery : IRequest<IEnumerable<PropertyDto>>
    {
        public int OwnerId { get; set; }
        public GetPropertiesByOwnerQuery(int ownerId)
        {
            OwnerId = ownerId;
        }
    }
}