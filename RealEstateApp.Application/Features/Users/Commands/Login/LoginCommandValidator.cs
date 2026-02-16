using FluentValidation;

namespace RealEstateApp.Application.Features.Users.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email required")
                .EmailAddress().WithMessage("The email format is incorrect");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password reqired");
        }
    }
}