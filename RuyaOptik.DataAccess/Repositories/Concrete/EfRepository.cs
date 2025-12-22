using Microsoft.EntityFrameworkCore;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using System.Linq.Expressions;

namespace RuyaOptik.DataAccess.Repositories.Concrete
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly RuyaOptikDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EfRepository(RuyaOptikDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // BASIC

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        // QUERY (Filter / Sort / Pagination)
        public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return _dbSet
                .Where(predicate)
                .AsQueryable();
        }

        // CRUD

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
