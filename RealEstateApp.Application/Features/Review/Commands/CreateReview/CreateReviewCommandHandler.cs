using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.DTOs.Review;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Review.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly ILogger<CreateReviewCommandHandler> _logger;
        private readonly INotificationService _notificationService;
        
        public CreateReviewCommandHandler(IUnitOfWork unitOfWork, ICacheService cache, ILogger<CreateReviewCommandHandler> logger, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
            _notificationService = notificationService;
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

            await _notificationService.SendNotificationAsync(
                userId: property.OwnerId,
                title: "New Review",
                message: $"{user.FirstName} {user.LastName} left a {request.Rating}-star review on '{property.Title}'.",
                type: "NewReview"
            );

            _logger.LogInformation("Review created for Property: {PropertyId} by User: {UserId}", review.PropertyId, review.UserId);

            await _cache.RemoveAsync($"reviews_property_{review.PropertyId}");
            await _cache.RemoveAsync($"property_{review.PropertyId}");

            await _cache.RemoveAsync("admin_stats");


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