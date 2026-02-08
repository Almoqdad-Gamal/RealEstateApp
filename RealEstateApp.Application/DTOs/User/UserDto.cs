using RealEstateApp.Domain.Enums;

namespace RealEstateApp.Application.DTOs.User
{
    public class UserDto
    {
        public int ID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}