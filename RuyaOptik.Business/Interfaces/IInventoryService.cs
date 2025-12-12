using RuyaOptik.DTO.Inventory;

namespace RuyaOptik.Business.Interfaces
{
    public interface IInventoryService
    {
        Task<InventoryDto?> GetByProductIdAsync(int productId);
        Task<InventoryDto> CreateAsync(InventoryCreateDto dto);
        Task<bool> UpdateAsync(int id, InventoryUpdateDto dto);
    }
}
