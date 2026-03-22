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
            // ------------ Seed Admin ---------------------------------------------------------------------
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

            // ------------ Seed Owners ---------------------------------------------------------------------
            var owners = new List<User>
            {
                new User
                {
                    FirstName = "Ahmed",
                    LastName = "Hassan",
                    Email = "ahmed.owner@realestate.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Owner@12345"),
                    PhoneNumber = "+201001234567",
                    Role = UserRole.Owner
                },
                new User
                {
                    FirstName = "Sara",
                    LastName = "Mohamed",
                    Email = "sara.owner@realestate.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Owner@12345"),
                    PhoneNumber = "+201009876543",
                    Role = UserRole.Owner
                }
            };
                
            await unitOfWork.Users.AddRangeAsync(owners);
            await unitOfWork.SaveChangesAsync();

            // ------------ Seed Clients ---------------------------------------------------------------------
            var clients = new List<User>
            {
                new User
                {
                    FirstName = "Omar",
                    LastName = "Ali",
                    Email = "omar.client@realestate.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Client@12345"),
                    PhoneNumber = "+201111234567",
                    Role = UserRole.Client
                },
                new User
                {
                    FirstName = "Nour",
                    LastName = "Khaled",
                    Email = "nour.client@realestate.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Client@12345"),
                    PhoneNumber = "+201119876543",
                    Role = UserRole.Client
                },
                new User
                {
                    FirstName = "Youssef",
                    LastName = "Ibrahim",
                    Email = "youssef.client@realestate.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Client@12345"),
                    PhoneNumber = "+201112345678",
                    Role = UserRole.Client
                }
            };

            await unitOfWork.Users.AddRangeAsync(clients);
            await unitOfWork.SaveChangesAsync();

            // ------------ Seed Properties ---------------------------------------------------------------------
            var properties = new List<Property>
            {
                // Owner 1 Properties
                new Property
                {
                    Title = "Modern Apartment in Zamalek",
                    Description = "Luxurious 3-bedroom apartment with Nile view in the heart of Zamalek.",
                    Type = PropertyType.Apartment,
                    ListingType = ListingType.Rent,
                    Status = PropertyStatus.Available,
                    Price = 25000,
                    Area = 180,
                    Address = "15 Hassan Sabry St",
                    City = "Cairo",
                    Country = "Egypt",
                    Latitude = 30.0650,
                    Longitude = 31.2195,
                    Bedrooms = 3,
                    Bathrooms = 2,
                    ParkingSpaces = 1,
                    Floor = 5,
                    YearBuilt = 2018,
                    HasGarden = false,
                    HasPool = false,
                    HasGym = true,
                    HasSecurity = true,
                    IsFurnished = true,
                    OwnerId = owners[0].Id
                },
                new Property
                {
                    Title = "Villa in New Cairo",
                    Description = "Spacious 5-bedroom villa with private pool and garden in New Cairo.",
                    Type = PropertyType.Villa,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Available,
                    Price = 8500000,
                    Area = 500,
                    Address = "Fifth Settlement, Street 90",
                    City = "Cairo",
                    Country = "Egypt",
                    Latitude = 30.0131,
                    Longitude = 31.4671,
                    Bedrooms = 5,
                    Bathrooms = 4,
                    ParkingSpaces = 3,
                    Floor = 0,
                    YearBuilt = 2020,
                    HasGarden = true,
                    HasPool = true,
                    HasGym = false,
                    HasSecurity = true,
                    IsFurnished = false,
                    OwnerId = owners[0].Id
                },
                new Property
                {
                    Title = "Studio in Maadi",
                    Description = "Cozy furnished studio perfect for singles or couples in Maadi.",
                    Type = PropertyType.Studio,
                    ListingType = ListingType.Rent,
                    Status = PropertyStatus.Available,
                    Price = 8000,
                    Area = 55,
                    Address = "Road 9, Maadi",
                    City = "Cairo",
                    Country = "Egypt",
                    Latitude = 29.9602,
                    Longitude = 31.2569,
                    Bedrooms = 0,
                    Bathrooms = 1,
                    ParkingSpaces = 0,
                    Floor = 2,
                    YearBuilt = 2015,
                    HasGarden = false,
                    HasPool = false,
                    HasGym = false,
                    HasSecurity = false,
                    IsFurnished = true,
                    OwnerId = owners[0].Id
                },
                new Property
                {
                    Title = "Penthouse in Sheikh Zayed",
                    Description = "Stunning penthouse with panoramic views and rooftop terrace.",
                    Type = PropertyType.Pentahouse,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Available,
                    Price = 12000000,
                    Area = 350,
                    Address = "Beverly Hills Compound",
                    City = "Giza",
                    Country = "Egypt",
                    Latitude = 30.0098,
                    Longitude = 31.0106,
                    Bedrooms = 4,
                    Bathrooms = 3,
                    ParkingSpaces = 2,
                    Floor = 15,
                    YearBuilt = 2022,
                    HasGarden = false,
                    HasPool = true,
                    HasGym = true,
                    HasSecurity = true,
                    IsFurnished = true,
                    OwnerId = owners[0].Id
                },
                new Property
                {
                    Title = "Townhouse in Katameya",
                    Description = "Beautiful townhouse in a gated community with all amenities.",
                    Type = PropertyType.Townhouse,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Available,
                    Price = 4500000,
                    Area = 280,
                    Address = "Katameya Heights",
                    City = "Cairo",
                    Country = "Egypt",
                    Latitude = 29.9980,
                    Longitude = 31.4397,
                    Bedrooms = 3,
                    Bathrooms = 3,
                    ParkingSpaces = 2,
                    Floor = 0,
                    YearBuilt = 2019,
                    HasGarden = true,
                    HasPool = true,
                    HasGym = true,
                    HasSecurity = true,
                    IsFurnished = false,
                    OwnerId = owners[0].Id
                },

                // Owner 2 Properties
                new Property
                {
                    Title = "Office Space in Downtown",
                    Description = "Prime office space in Cairo Downtown, fully equipped.",
                    Type = PropertyType.Office,
                    ListingType = ListingType.Rent,
                    Status = PropertyStatus.Available,
                    Price = 35000,
                    Area = 200,
                    Address = "Talaat Harb Square",
                    City = "Cairo",
                    Country = "Egypt",
                    Latitude = 30.0461,
                    Longitude = 31.2394,
                    Bedrooms = 0,
                    Bathrooms = 2,
                    ParkingSpaces = 5,
                    Floor = 3,
                    YearBuilt = 2010,
                    HasGarden = false,
                    HasPool = false,
                    HasGym = false,
                    HasSecurity = true,
                    IsFurnished = true,
                    OwnerId = owners[1].Id
                },
                new Property
                {
                    Title = "Apartment in Alexandria",
                    Description = "Sea-view apartment on the Corniche in Alexandria.",
                    Type = PropertyType.Apartment,
                    ListingType = ListingType.Rent,
                    Status = PropertyStatus.Available,
                    Price = 15000,
                    Area = 140,
                    Address = "Corniche Road, Ramleh",
                    City = "Alexandria",
                    Country = "Egypt",
                    Latitude = 31.2001,
                    Longitude = 29.9187,
                    Bedrooms = 2,
                    Bathrooms = 2,
                    ParkingSpaces = 1,
                    Floor = 8,
                    YearBuilt = 2016,
                    HasGarden = false,
                    HasPool = false,
                    HasGym = false,
                    HasSecurity = true,
                    IsFurnished = true,
                    OwnerId = owners[1].Id
                },
                new Property
                {
                    Title = "Shop in Heliopolis",
                    Description = "Commercial shop in a busy area in Heliopolis.",
                    Type = PropertyType.Shop,
                    ListingType = ListingType.Rent,
                    Status = PropertyStatus.Available,
                    Price = 20000,
                    Area = 80,
                    Address = "Merghany Street",
                    City = "Cairo",
                    Country = "Egypt",
                    Latitude = 30.0870,
                    Longitude = 31.3195,
                    Bedrooms = 0,
                    Bathrooms = 1,
                    ParkingSpaces = 0,
                    Floor = 0,
                    YearBuilt = 2008,
                    HasGarden = false,
                    HasPool = false,
                    HasGym = false,
                    HasSecurity = false,
                    IsFurnished = false,
                    OwnerId = owners[1].Id
                },
                new Property
                {
                    Title = "Villa in North Coast",
                    Description = "Summer villa with beach access in the North Coast.",
                    Type = PropertyType.Villa,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Available,
                    Price = 6500000,
                    Area = 320,
                    Address = "Hacienda Bay",
                    City = "North Coast",
                    Country = "Egypt",
                    Latitude = 31.0194,
                    Longitude = 28.0461,
                    Bedrooms = 4,
                    Bathrooms = 3,
                    ParkingSpaces = 2,
                    Floor = 0,
                    YearBuilt = 2021,
                    HasGarden = true,
                    HasPool = true,
                    HasGym = false,
                    HasSecurity = true,
                    IsFurnished = false,
                    OwnerId = owners[1].Id
                },
                new Property
                {
                    Title = "Land in 6th of October",
                    Description = "Residential land ready for construction in 6th of October City.",
                    Type = PropertyType.Land,
                    ListingType = ListingType.Sale,
                    Status = PropertyStatus.Available,
                    Price = 3000000,
                    Area = 600,
                    Address = "12th District",
                    City = "Giza",
                    Country = "Egypt",
                    Bedrooms = 0,
                    Bathrooms = 0,
                    HasGarden = false,
                    HasPool = false,
                    HasGym = false,
                    HasSecurity = false,
                    IsFurnished = false,
                    OwnerId = owners[1].Id
                }
            };

            await unitOfWork.Properties.AddRangeAsync(properties);
            await unitOfWork.SaveChangesAsync();

            // ------------ Seed Booking ---------------------------------------------------------------------

            var bookings = new List<Booking>
            {
                new Booking
                {
                    PropertyId = properties[0].Id,
                    ClientId = clients[0].Id,
                    BookingDate = DateTime.UtcNow.AddDays(3),
                    BookingTime = new TimeSpan(10, 0, 0),
                    Status = BookingStatus.Confirmed,
                    Notes = "Please prepare the documents for viewing."
                },
                new Booking
                {
                    PropertyId = properties[1].Id,
                    ClientId = clients[0].Id,
                    BookingDate = DateTime.UtcNow.AddDays(5),
                    BookingTime = new TimeSpan(14, 0, 0),
                    Status = BookingStatus.Pending,
                    Notes = "Interested in buying, need full tour."
                },
                new Booking
                {
                    PropertyId = properties[2].Id,
                    ClientId = clients[1].Id,
                    BookingDate = DateTime.UtcNow.AddDays(2),
                    BookingTime = new TimeSpan(11, 0, 0),
                    Status = BookingStatus.Pending
                },
                new Booking
                {
                    PropertyId = properties[6].Id,
                    ClientId = clients[1].Id,
                    BookingDate = DateTime.UtcNow.AddDays(7),
                    BookingTime = new TimeSpan(16, 0, 0),
                    Status = BookingStatus.Confirmed
                },
                new Booking
                {
                    PropertyId = properties[4].Id,
                    ClientId = clients[2].Id,
                    BookingDate = DateTime.UtcNow.AddDays(1),
                    BookingTime = new TimeSpan(9, 0, 0),
                    Status = BookingStatus.Completed,
                    Notes = "Second visit."
                },
                new Booking
                {
                    PropertyId = properties[8].Id,
                    ClientId = clients[2].Id,
                    BookingDate = DateTime.UtcNow.AddDays(10),
                    BookingTime = new TimeSpan(13, 0, 0),
                    Status = BookingStatus.Pending
                }
            };

            await unitOfWork.Bookings.AddRangeAsync(bookings);
            await unitOfWork.SaveChangesAsync();

            // ------------ Seed Reviews ---------------------------------------------------------------------
            var reviews = new List<Review>
            {
                new Review
                {
                    PropertyId = properties[0].Id,
                    UserId = clients[0].Id,
                    Rating = 5,
                    Comment = "Amazing apartment with stunning Nile views. Highly recommended!"
                },
                new Review
                {
                    PropertyId = properties[2].Id,
                    UserId = clients[1].Id,
                    Rating = 4,
                    Comment = "Great studio for the price. Very clean and well-maintained."
                },
                new Review
                {
                    PropertyId = properties[4].Id,
                    UserId = clients[2].Id,
                    Rating = 5,
                    Comment = "Beautiful townhouse in a great location. Worth every penny."
                },
                new Review
                {
                    PropertyId = properties[6].Id,
                    UserId = clients[0].Id,
                    Rating = 4,
                    Comment = "Nice sea view apartment. The location is perfect."
                },
                new Review
                {
                    PropertyId = properties[8].Id,
                    UserId = clients[1].Id,
                    Rating = 3,
                    Comment = "Good villa but needs some renovation. Price is fair."
                },
                new Review
                {
                    PropertyId = properties[1].Id,
                    UserId = clients[2].Id,
                    Rating = 5,
                    Comment = "Incredible villa with everything you need. Dream home!"
                }
            };

            await unitOfWork.Bookings.AddRangeAsync(bookings);
            await unitOfWork.SaveChangesAsync();
        }
    }
}