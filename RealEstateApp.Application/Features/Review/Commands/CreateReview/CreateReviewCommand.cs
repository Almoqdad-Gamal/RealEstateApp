using MediatR;
using RealEstateApp.Application.DTOs.Review;

namespace RealEstateApp.Application.Features.Review.Commands.CreateReview
{
    public class CreateReviewCommand : IRequest<ReviewDto>
    {
        public int PropertyId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}