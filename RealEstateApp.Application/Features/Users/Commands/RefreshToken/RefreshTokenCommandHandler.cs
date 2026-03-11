using MediatR;
using RealEstateApp.Application.DTOs.User;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }
        public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Get the user by the refresh token
            var user = await _unitOfWork.Users.GetByRefreshToken(request.RefreshToken);

            if(user == null)
                throw new UnauthorizedException("Invalid refresh token");

            // Verify that the token is still valid
            if(user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new UnauthorizedException("Refresh token has expired. Please login again.");

            // Generate new tokens
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // Update the refresh token in the database
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiresAt =  DateTime.UtcNow.AddMinutes(15),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                User = new UserDto
                {
                    ID = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                }
            };
        }
    }
}