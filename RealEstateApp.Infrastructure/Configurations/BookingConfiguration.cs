using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Infrastructure.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Notes)
                .HasMaxLength(500);

            //Indexes
            builder.HasIndex(b => b.PropertyId);
            builder.HasIndex(b => b.ClientId);
            builder.HasIndex(b => b.BookingDate);
            builder.HasIndex(b => b.Status);

            //Composite Index
            builder.HasIndex(b => new {b.PropertyId, b.BookingDate});

            //Relationships
            builder.HasOne(b => b.Property)
            .WithMany(p => p.Bookings)
            .HasForeignKey(b => b.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Client)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}