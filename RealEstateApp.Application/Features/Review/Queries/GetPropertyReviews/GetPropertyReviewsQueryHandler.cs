using MediatR;
using RealEstateApp.Application.DTOs.Review;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Review.Queries.GetPropertyReviews
{
    public class GetPropertyReviewsQueryHandler : IRequestHandler<GetPropertyReviewsQuery, IEnumerable<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPropertyReviewsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReviewDto>> Handle(GetPropertyReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _unitOfWork.Reviews.GetPropertyReviewsAsync(request.PropertyId);

            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                PropertyId = r.PropertyId,
                UserId = r.UserId,
                UserName = $"{r.User.FirstName} {r.User.LastName}",
                CreatedAt = r.CreatedAt
            }).ToList();
        }
    }
}