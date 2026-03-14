using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.DTOs.User;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, ILogger<RegisterUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            //Check if email already exists
            if(await _unitOfWork.Users.EmailExistsAsync(request.Email))
            {
                throw new ConflictException("This email is already registered.");
            }

            //Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            

            //Create user entity
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PhoneNumber = request.PhoneNumber,
                Role = request.Role
            };


            //Add to repository
            await _unitOfWork.Users.AddAsync(user);

            _logger.LogInformation("New user registered with email: {Email}", request.Email);
            
            await _unitOfWork.SaveChangesAsync();

            //Map to DTO and return 
            return new UserDto
            {
                ID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }
    }
}