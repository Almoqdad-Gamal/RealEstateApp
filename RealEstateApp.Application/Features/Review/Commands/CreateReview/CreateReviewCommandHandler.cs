using MediatR;
using RealEstateApp.Application.DTOs.Review;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Review.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        
        public CreateReviewCommandHandler(IUnitOfWork unitOfWork, ICacheService cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }
        public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            // Check if the property is existing
            var property = await _unitOfWork.Properties.GetByIdAsync(request.PropertyId);
            if(property == null)
                throw new NotFoundException("Property", request.PropertyId);

            // Check if the user is existing
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if(user == null)
                throw new NotFoundException("User", request.UserId);

            // Check for any previous reviews
            var hasReviewed = await _unitOfWork.Reviews.HasUserReviewedPropertyAsync(request.UserId, request.PropertyId);
            if(hasReviewed)
                throw new ConflictException("You have already reviewed this property.");

            // Create review
            var review = new Domain.Entities.Review
            {
                PropertyId = request.PropertyId,
                UserId = request.UserId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            await _cache.RemoveAsync($"reviews_property_{review.PropertyId}");
            await _cache.RemoveAsync($"property_{review.PropertyId}");


            return new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                PropertyId = review.PropertyId,
                UserId = review.UserId,
                UserName = $"{user.FirstName} {user.LastName}",
                CreatedAt = review.CreatedAt
            };
        }
    }
}