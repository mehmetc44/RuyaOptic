using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using RuyaOptik.Business.Consts;
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
        private readonly IMemoryCache _cache;

        public InventoryService(
            IInventoryRepository inventoryRepository,
            IMapper mapper,
            IMemoryCache cache)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<InventoryDto?> GetByProductIdAsync(int productId)
        {
            var key = CacheKeys.Inventory_ByProduct(productId);

            if (_cache.TryGetValue(key, out InventoryDto cached))
                return cached;

            var inventory = await _inventoryRepository.GetByProductIdAsync(productId);
            if (inventory == null) return null;

            var dto = _mapper.Map<InventoryDto>(inventory);

            _cache.Set(key, dto, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

            return dto;
        }

        public async Task CreateAsync(InventoryCreateDto dto)
        {
            var existingInventory = await _inventoryRepository.GetByProductIdAsync(dto.ProductId);
            if (existingInventory != null)
                throw new Exception("This product already has inventory.");

            var inventory = _mapper.Map<Inventory>(dto);

            await _inventoryRepository.AddAsync(inventory);
            await _inventoryRepository.SaveChangesAsync();

            // invalidate
            _cache.Remove(CacheKeys.Inventory_ByProduct(dto.ProductId));
        }

        public async Task<bool> UpdateAsync(int id, InventoryUpdateDto dto)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(id);
            if (inventory == null || inventory.IsDeleted)
                return false;

            _mapper.Map(dto, inventory);
            await _inventoryRepository.UpdateAsync(inventory);
            await _inventoryRepository.SaveChangesAsync();

            // invalidate
            _cache.Remove(CacheKeys.Inventory_ByProduct(inventory.ProductId));

            return true;
        }
    }
}
