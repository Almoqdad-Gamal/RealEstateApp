using MediatR;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Users.Commands.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public ForgetPasswordCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        public async Task Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);

            // Even if the email doesn't exist, we don't tell the user
            // This is to prevent anyone from knowing the registered emails.

            if (user == null) return;

            // Generate a random token
            var resetToken = Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(64));

            // Store the token in the database with its expiration date
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            // Send the email
            await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken);
        }
    }
}