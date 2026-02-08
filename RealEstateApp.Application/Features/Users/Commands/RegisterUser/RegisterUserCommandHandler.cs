using MediatR;
using RealEstateApp.Application.DTOs.User;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            //Check if email already exists
            if(await _unitOfWork.Users.EmailExistsAsync(request.Email))
            {
                throw new Exception("Email already exists");
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