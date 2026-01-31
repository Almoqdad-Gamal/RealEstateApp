using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}


        //Dbsets
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Apply all configration from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            //Global query filter - Soft Delete

            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Property>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<PropertyImage>().HasQueryFilter(pi => !pi.IsDeleted);
            modelBuilder.Entity<Booking>().HasQueryFilter(b => !b.IsDeleted);
            modelBuilder.Entity<Review>().HasQueryFilter(r => !r.IsDeleted);
        }

        //Override SaveChanges to update UpdatedAt Automaticly
        public override Task<int> SaveChangesAsync (CancellationToken cancellationToken = default)
        {
            var entries  = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if(entry.Entity is Domain.Common.BaseEntity entity)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}