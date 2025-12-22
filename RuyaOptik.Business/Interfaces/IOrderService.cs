using RuyaOptik.DTO.Order;
using RuyaOptik.Entity.Enums;

namespace RuyaOptik.Business.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(OrderCreateDto dto);
        Task<List<OrderDto>> GetByUserIdAsync(string userId);

        Task<bool> UpdateStatusAsync(int orderId, OrderStatus status);
    }
}
