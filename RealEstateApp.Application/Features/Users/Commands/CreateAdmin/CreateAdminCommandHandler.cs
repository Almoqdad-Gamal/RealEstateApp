using MediatR;
using Microsoft.Extensions.Logging;
using RealEstateApp.Application.DTOs.User;
using RealEstateApp.Application.Exceptions;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.Features.Users.Commands.CreateAdmin
{
    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateAdminCommandHandler> _logger;

        public CreateAdminCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateAdminCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<UserDto> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
        {
            if(await _unitOfWork.Users.EmailExistsAsync(request.Email))
                throw new ConflictException("This email is already registered.");

            var admin = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
                Role = UserRole.Admin
            };

            await _unitOfWork.Users.AddAsync(admin);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("New admin created with email: {Email}", admin.Email);

            return new UserDto
            {
                ID = admin.Id,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                PhoneNumber = admin.PhoneNumber,
                Role = admin.Role,
                CreatedAt = admin.CreatedAt
            };
        }
    }
}