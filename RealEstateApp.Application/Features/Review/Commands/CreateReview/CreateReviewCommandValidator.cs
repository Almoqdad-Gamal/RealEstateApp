using FluentValidation;

namespace RealEstateApp.Application.Features.Review.Commands.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("The rating should be between 1 and 5");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment required")
                .MaximumLength(1000).WithMessage("Comment is too long");

            RuleFor(x => x.PropertyId)
                .GreaterThan(0).WithMessage("Property ID is requierd");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID is requierd");
        }
    }
}