using RuyaOptik.DTO.Order;

namespace RuyaOptik.Business.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(OrderCreateDto dto);
        Task<List<OrderDto>> GetByUserIdAsync(string userId);
    }
}
