using RuyaOptik.Entity.Entities.Concrete;

namespace RuyaOptik.DataAccess.Repositories.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task AddCartItemAsync(CartItem item);
        Task DeleteCartItemAsync(CartItem item);
    }
}
