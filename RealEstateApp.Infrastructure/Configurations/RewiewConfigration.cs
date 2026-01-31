using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Infrastructure.Configurations
{
    public class RewiewConfigration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Comment)
            .IsRequired()
            .HasMaxLength(1000);

            builder.Property(r => r.Rating)
            .IsRequired();

            //Indexes
            builder.HasIndex(r => r.PropertyId);
            builder.HasIndex(r => r.UserId);

            //Composite Index
            builder.HasIndex(r => new {r.PropertyId, r.UserId})
            .IsUnique();

            //Relationships
            builder.HasOne(r => r.Property)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}