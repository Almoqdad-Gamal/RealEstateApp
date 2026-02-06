using System.Linq.Expressions;

namespace RealEstateApp.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        //Get Operations
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        //Add Operations
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entites);

        //Update Operations
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        //Delete Operations
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        //Query Operations
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    }
}