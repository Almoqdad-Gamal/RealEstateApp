using MediatR;
using RealEstateApp.Application.DTOs.Admin;

namespace RealEstateApp.Application.Features.Admin.Queries.GetAdminStats
{
    public class GetAdminStatsQuery : IRequest<AdminStatsDto>
    {
    }
}