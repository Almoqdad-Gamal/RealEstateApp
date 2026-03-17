using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Review.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly ILogger<DeleteReviewCommandHandler> _logger;

        public DeleteReviewCommandHandler(IUnitOfWork unitOfWork, ICacheService cache, ILogger<DeleteReviewCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }
        public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(request.ReviewId);

            if(review == null)
                throw new NotFoundException("Review", request.ReviewId);

            // Only reviewer can delete the review.
            if(review.UserId != request.RequestingUserId)
                throw new UnauthorizedException("You can only delete your own reviews.");

            _unitOfWork.Reviews.Delete(review);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Review {ReviewId} deleted by User {UserId}.", request.ReviewId, request.RequestingUserId);

            await _cache.RemoveAsync($"reviews_property_{review.PropertyId}");
            await _cache.RemoveAsync($"property_{review.PropertyId}");
            
        }
    }
}