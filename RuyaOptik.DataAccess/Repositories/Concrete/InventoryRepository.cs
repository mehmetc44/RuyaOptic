using Microsoft.EntityFrameworkCore;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.Entity.Entities.Concrete;

namespace RuyaOptik.DataAccess.Repositories.Concrete
{
    public class InventoryRepository
        : EfRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(RuyaOptikDbContext context)
            : base(context)
        {
        }

        public async Task<Inventory?> GetByProductIdAsync(int productId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.ProductId == productId && !x.IsDeleted);
        }
    }
}
