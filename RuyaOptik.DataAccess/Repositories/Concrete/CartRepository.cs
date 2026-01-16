using Microsoft.EntityFrameworkCore;
using RuyaOptik.DataAccess.Context;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.Entity.Concrete;

namespace RuyaOptik.DataAccess.Repositories.Concrete
{
    public class CartRepository : EfRepository<Cart>, ICartRepository
    {
        private readonly RuyaOptikDbContext _db;

        public CartRepository(RuyaOptikDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            return await _db.Carts
                .Include(c => c.Items)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            return await _db.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci =>
                    ci.CartId == cartId &&
                    ci.ProductId == productId &&
                    !ci.IsDeleted);
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
        {
            return await _db.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && !ci.IsDeleted);
        }

        public async Task AddCartItemAsync(CartItem item)
        {
            await _db.CartItems.AddAsync(item);
        }

        public Task DeleteCartItemAsync(CartItem item)
        {
            _db.CartItems.Remove(item);
            return Task.CompletedTask;
        }
    }
}
