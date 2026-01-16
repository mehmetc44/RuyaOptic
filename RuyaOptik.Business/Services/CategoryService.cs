using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using RuyaOptik.Business.Consts;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DTO.Category;
using RuyaOptik.Entity.Concrete;

namespace RuyaOptik.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly CacheVersionService _version;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IMemoryCache cache,
            CacheVersionService version)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _cache = cache;
            _version = version;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var v = _version.Get(CacheKeys.Category_Version);
            var key = CacheKeys.Categories_All(v);

            if (_cache.TryGetValue(key, out List<CategoryDto> cached))
                return cached;

            var categories = await _categoryRepository.GetWhereAsync(x => !x.IsDeleted);
            var dto = _mapper.Map<List<CategoryDto>>(categories);

            _cache.Set(key, dto, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            });

            return dto;
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null || category.IsDeleted) return null;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
        {
            var entity = _mapper.Map<Category>(dto);

            await _categoryRepository.AddAsync(entity);
            await _categoryRepository.SaveChangesAsync();

            // invalidate ALL category lists
            _version.Increment(CacheKeys.Category_Version);

            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, CategoryUpdateDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null || category.IsDeleted) return false;

            _mapper.Map(dto, category);
            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();

            // invalidate
            _version.Increment(CacheKeys.Category_Version);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null || category.IsDeleted) return false;

            category.IsDeleted = true;
            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();

            // invalidate
            _version.Increment(CacheKeys.Category_Version);

            return true;
        }
    }
}
