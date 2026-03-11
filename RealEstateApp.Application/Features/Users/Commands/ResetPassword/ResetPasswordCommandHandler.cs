using MediatR;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResetPasswordCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Get the user by the token
            var user = await _unitOfWork.Users.GetByResetToken(request.Token);

            if (user == null)
                throw new BadRequestException("Invalid or expired reset token.");

            // Change the password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            // Delete the token so it can't be used again
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}