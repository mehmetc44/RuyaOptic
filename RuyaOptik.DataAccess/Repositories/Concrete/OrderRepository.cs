using Microsoft.EntityFrameworkCore;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.Entity.Entities.Concrete;

namespace RuyaOptik.DataAccess.Repositories.Concrete
{
    public class OrderRepository : EfRepository<Order>, IOrderRepository
    {
        public OrderRepository(RuyaOptikDbContext context) : base(context)
        {
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId && !o.IsDeleted)
                .ToListAsync();
        }
    }
}
