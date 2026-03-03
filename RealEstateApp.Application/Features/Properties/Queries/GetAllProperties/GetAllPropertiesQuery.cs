using MediatR;
using RealEstateApp.Application.DTOs.Property;
using RealEstateApp.Application.Models;

namespace RealEstateApp.Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQuery : IRequest<PaginatedResult<PropertyDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}