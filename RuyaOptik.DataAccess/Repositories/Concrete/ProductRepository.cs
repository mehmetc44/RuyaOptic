using Microsoft.EntityFrameworkCore;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.Entity.Entities.Concrete;

namespace RuyaOptik.DataAccess.Repositories.Concrete
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(RuyaOptikDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetActiveProductsAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
