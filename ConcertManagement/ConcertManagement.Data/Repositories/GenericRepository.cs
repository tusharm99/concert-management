using ConcertManagement.Core.Entities;
using ConcertManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ConcertManagement.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        protected readonly CmDbContext _context;
        protected readonly DbSet<T> _entities;

        public GenericRepository(CmDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entities = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> AddAsync(T item)
        {
            await _entities.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task RemoveAsync(T item)
        {
            _entities.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T item)
        {
            _entities.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
    }
}
