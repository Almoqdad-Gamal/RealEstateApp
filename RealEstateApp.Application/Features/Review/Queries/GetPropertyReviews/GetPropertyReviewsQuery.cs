using MediatR;
using RealEstateApp.Application.DTOs.Review;

namespace RealEstateApp.Application.Features.Review.Queries.GetPropertyReviews
{
    public class GetPropertyReviewsQuery : IRequest<IEnumerable<ReviewDto>>
    {
        public int PropertyId { get; set; }
        public GetPropertyReviewsQuery(int propertyId)
        {
            PropertyId = propertyId;
        }
    }
}