using MediatR;
using RealEstateApp.Application.DTOs.Review;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Review.Queries.GetPropertyReviews
{
    public class GetPropertyReviewsQueryHandler : IRequestHandler<GetPropertyReviewsQuery, IEnumerable<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        public GetPropertyReviewsQueryHandler(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<IEnumerable<ReviewDto>> Handle(GetPropertyReviewsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"reviews_property_{request.PropertyId}";
            
            var cached = await _cache.GetAsync<IEnumerable<ReviewDto>>(cacheKey);
            if(cached != null)
                return cached;

            var reviews = await _unitOfWork.Reviews.GetPropertyReviewsAsync(request.PropertyId);

            var reviewsDto = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                PropertyId = r.PropertyId,
                UserId = r.UserId,
                UserName = $"{r.User.FirstName} {r.User.LastName}",
                CreatedAt = r.CreatedAt
            }).ToList();

            await _cache.SetAsync(cacheKey, reviewsDto, TimeSpan.FromMinutes(5));

            return reviewsDto;
        }
    }
}