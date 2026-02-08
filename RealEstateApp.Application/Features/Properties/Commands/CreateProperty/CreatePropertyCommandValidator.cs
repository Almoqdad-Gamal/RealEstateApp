using FluentValidation;

namespace RealEstateApp.Application.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("Area must be greater than 0");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(300).WithMessage("Address must not exceed 300 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(100).WithMessage("Country must not exceed 100 characters");

            RuleFor(x => x.Bedrooms)
                .GreaterThanOrEqualTo(0).WithMessage("Bedrooms must be 0 or greater")
                .LessThanOrEqualTo(20).WithMessage("Bedrooms must not exceed 20");

            RuleFor(x => x.Bathrooms)
                .GreaterThanOrEqualTo(0).WithMessage("Bathrooms must be 0 or greater")
                .LessThanOrEqualTo(20).WithMessage("Bathrooms must not exceed 20");

            RuleFor(x => x.OwnerId)
                .GreaterThan(0).WithMessage("Owner ID is required");
        }
    }
}