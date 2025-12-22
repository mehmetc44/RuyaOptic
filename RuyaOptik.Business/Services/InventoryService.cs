using AutoMapper;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DTO.Inventory;
using RuyaOptik.Entity.Concrete;

namespace RuyaOptik.Business.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public InventoryService(
            IInventoryRepository inventoryRepository,
            IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<InventoryDto?> GetByProductIdAsync(int productId)
        {
            var inventory = await _inventoryRepository.GetByProductIdAsync(productId);
            if (inventory == null) return null;

            return _mapper.Map<InventoryDto>(inventory);
        }

        public async Task CreateAsync(InventoryCreateDto dto)
        {
            var existingInventory = await _inventoryRepository
                .GetByProductIdAsync(dto.ProductId);

            if (existingInventory != null)
                throw new Exception("This product already has inventory.");

            var inventory = _mapper.Map<Inventory>(dto);

            await _inventoryRepository.AddAsync(inventory);
            await _inventoryRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, InventoryUpdateDto dto)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(id);
            if (inventory == null || inventory.IsDeleted)
                return false;

            _mapper.Map(dto, inventory);
            await _inventoryRepository.UpdateAsync(inventory);
            await _inventoryRepository.SaveChangesAsync();

            return true;
        }
    }
}
