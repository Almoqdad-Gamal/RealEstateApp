using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            
            builder.HasKey(u => u.Id);

            //Properties
            builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

            builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

            builder.Property(u => u.ProfileImageUrl)
            .HasMaxLength(500);

            //Indexes
            //Email must me unique
            builder.HasIndex(u => u.Email)
            .IsUnique(); 

            //Relationships
            builder.HasMany(u => u.Properties)
            .WithOne(p => p.Owner)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict); //It will not delete the user if you delete the properties

            builder.HasMany(u => u.Bookings)
            .WithOne(b => b.Client)
            .HasForeignKey(b => b.ClientId)
            .OnDelete(DeleteBehavior.Cascade); // If you delete the user it will be delete the bookings

            builder.HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}