using MediatR;
using RealEstateApp.Application.DTOs.Property;

namespace RealEstateApp.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQuery : IRequest<IEnumerable<PropertyDto>>
    {}
}Get