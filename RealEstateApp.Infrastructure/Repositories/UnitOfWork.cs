using Microsoft.EntityFrameworkCore.Storage;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Data;

namespace RealEstateApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;
        
        public IUserRepository Users {get;}
        public IPropertyRepository Properties {get;}
        public IBookingRepository Bookings {get;}
        public IReviewRepository Reviews {get;}
        public IGenericRepository<PropertyImage> PropertyImages {get;}

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            //Initialize repositories
            Users = new UserRepository(_context);
            Properties = new PropertyRepository(_context);
            Bookings = new BookingRepository(_context);
            Reviews = new ReviewRepository(_context);
            PropertyImages = new GenericRepository<PropertyImage>(_context);

        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                if(_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch 
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if(_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}