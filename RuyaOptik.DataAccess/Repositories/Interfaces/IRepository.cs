using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RuyaOptik.DataAccess.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Table { get; }
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

        // FILTER / SORT / PAGINATION
        IQueryable<T> Query(Expression<Func<T, bool>> predicate);

        // CRUD
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task<int> SaveChangesAsync();
    }
}
