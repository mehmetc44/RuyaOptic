using RuyaOptik.DTO.Cart;

namespace RuyaOptik.Business.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(string userId);
        Task<CartDto> AddToCartAsync(AddToCartDto dto);
        Task<bool> RemoveItemAsync(int cartItemId);
        Task<bool> ClearCartAsync(string userId);
    }
}
