using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Infrastructure.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("Properties");

            builder.HasKey(p => p.Id);

            //Properties
            builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

            builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(2000);

            builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Area)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

            builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(300);

            builder.Property(p => p.City)
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(p => p.Country)
            .IsRequired()
            .HasMaxLength(100);

            //Indexes
            builder.HasIndex(p => p.City);
            builder.HasIndex(p => p.Type);
            builder.HasIndex(p => p.ListingType);
            builder.HasIndex(p => p.Status);
            builder.HasIndex(p => p.Price);

            //Composite Index
            builder.HasIndex(p => new {p.City, p.Type, p.ListingType});

            //Relationships
            builder.HasOne(p => p.Owner)
            .WithMany(u => u.Properties)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Images)
            .WithOne(pi => pi.Property)
            .HasForeignKey(pi => pi.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Bookings)
            .WithOne(b => b.Property)
            .HasForeignKey(b => b.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Reviews)
            .WithOne(r => r.Property)
            .HasForeignKey(r => r.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
        }

    }
}