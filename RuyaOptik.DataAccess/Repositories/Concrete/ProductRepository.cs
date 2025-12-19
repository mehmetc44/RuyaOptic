using Microsoft.EntityFrameworkCore;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.Entity.Entities.Concrete;
using System.Linq.Expressions;

namespace RuyaOptik.DataAccess.Repositories.Concrete
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(RuyaOptikDbContext context) : base(context) { }

        public async Task<List<Product>> GetActiveProductsAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public async Task<List<Product>> GetPagedAsync(
            Expression<Func<Product, bool>> predicate,
            int skip,
            int take)
        {
            return await _dbSet
                .Where(predicate)
                .OrderByDescending(p => p.CreatedDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
