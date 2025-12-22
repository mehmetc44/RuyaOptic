using AutoMapper;
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

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetWhereAsync(x => !x.IsDeleted);
            return _mapper.Map<List<CategoryDto>>(categories);
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


            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, CategoryUpdateDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null || category.IsDeleted) return false;

            _mapper.Map(dto, category);
            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null || category.IsDeleted) return false;

            // Soft delete
            category.IsDeleted = true;
            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return true;
        }
    }
}
