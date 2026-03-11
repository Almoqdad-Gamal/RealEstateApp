using MediatR;
using RealEstateApp.Application.DTOs.User;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;

namespace RealEstateApp.Application.Features.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }
        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Search the user by his email
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if(user == null)
                throw new UnauthorizedException("Incorrect email or password.");

            // Check the password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if(!isPasswordValid)
                throw new UnauthorizedException("Incorrect email or password.");

            // Create jwt token
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Save the refresh token in the database
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            // Return token with user data
            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15),
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