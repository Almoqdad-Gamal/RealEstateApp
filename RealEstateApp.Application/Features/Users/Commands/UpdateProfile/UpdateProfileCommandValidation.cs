using FluentValidation;

namespace RealEstateApp.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandValidation : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name can't be exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name can't be exceed 50 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .MaximumLength(30).WithMessage("Phone number can't be exceed 30 numbers");
        }
    }
}