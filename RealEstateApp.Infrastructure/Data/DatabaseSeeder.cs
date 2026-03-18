using Microsoft.Extensions.Configuration;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            // Checking if their is any admin in the database
            var emailExists = await unitOfWork.Users.AnyAsync(u => u.Role == UserRole.Admin);
            if(emailExists) return;

            var adminSettings = configuration.GetSection("AdminSettings");

            var admin = new User
            {
                FirstName = adminSettings["FirstName"]!,
                LastName = adminSettings["LastName"]!,
                Email = adminSettings["Email"]!,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminSettings["Password"]!),
                PhoneNumber = adminSettings["PhoneNumber"]!,
                Role = UserRole.Admin
            };

            await unitOfWork.Users.AddAsync(admin);
            await unitOfWork.SaveChangesAsync();
        }
    }
}