using Microsoft.EntityFrameworkCore;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.Entity.Concrete;

namespace RuyaOptik.DataAccess.Repositories.Concrete
{
    public class InventoryRepository
        : EfRepository<Inventory>, IInventoryRepository
    {
        private readonly RuyaOptikDbContext _context;

        public InventoryRepository(RuyaOptikDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Inventory?> GetByProductIdAsync(int productId)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(x => x.ProductId == productId);
        }
    }
}
