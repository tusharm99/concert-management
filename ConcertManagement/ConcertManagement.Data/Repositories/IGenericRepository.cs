using ConcertManagement.Core.Entities;
using System.Linq.Expressions;

namespace ConcertManagement.Data.Repositories
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T item);
        Task RemoveAsync(T entity);
        Task UpdateAsync(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
