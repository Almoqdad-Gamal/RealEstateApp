using MediatR;

namespace RealEstateApp.Application.Features.Review.Commands.DeleteReview
{
    public class DeleteReviewCommand : IRequest
    {
        public int ReviewId { get; set; }
        public int RequestingUserId { get; set; }

        public DeleteReviewCommand(int reviewId)
        {
            ReviewId = reviewId;
        }
    }
}