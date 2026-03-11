using MediatR;

namespace RealEstateApp.Application.Features.Users.Commands.ForgetPassword
{
    public class ForgetPasswordCommand : IRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}