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

        // Aktif ürünler (mevcut kullanım)
        public async Task<List<Product>> GetActiveProductsAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive && !p.IsDeleted)
                .ToListAsync();
        }

        // TOPLAM ÜRÜN SAYISI (pagination)
        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync(p => !p.IsDeleted);
        }

        // PAGED LIST (pagination)
        public async Task<List<Product>> GetPagedAsync(int skip, int take)
        {
            return await _dbSet
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
