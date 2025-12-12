using AutoMapper;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DTO.Inventory;
using RuyaOptik.Entity.Entities.Concrete;

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

        public async Task<InventoryDto> CreateAsync(InventoryCreateDto dto)
        {
            var entity = _mapper.Map<Inventory>(dto);

            await _inventoryRepository.AddAsync(entity);
            await _inventoryRepository.SaveChangesAsync();

            return _mapper.Map<InventoryDto>(entity);
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
