using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Infrastructure.Configurations
{
    public class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.ToTable("PropertyImages");

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

            //Index
            builder.HasIndex(pi => pi.PropertyId);

            //Composite Index
            builder.HasIndex(pi => new {pi.PropertyId, pi.IsPrimary});

            builder.HasOne(pi => pi.Property)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}