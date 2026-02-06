using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Repositories
        IUserRepository Users { get;}
        IPropertyRepository Properties { get;}
        IBookingRepository Bookings { get;}
        IReviewRepository Reviews { get;}
        IGenericRepository<PropertyImage> PropertyImages { get;}

        //Save Changes
        Task<int> SaveChangesAsync();

        //Transaction Support
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}