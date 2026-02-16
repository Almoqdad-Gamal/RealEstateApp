using MediatR;
using RealEstateApp.Application.DTOs.Review;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Review.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CreateReviewCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            // Check if the property is existing
            var property = await _unitOfWork.Properties.GetByIdAsync(request.PropertyId);
            if(property == null)
                throw new Exception("Property not found");

            // Check if the user is existing
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if(user == null)
                throw new Exception("User not found");

            // Check for any previous reviews
            var hasReviewed = await _unitOfWork.Reviews.HasUserReviewedPropertyAsync(request.UserId, request.PropertyId);
            if(hasReviewed)
                throw new Exception("You have previously reviewed this property");

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