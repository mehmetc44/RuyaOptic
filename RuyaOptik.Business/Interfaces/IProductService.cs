using RuyaOptik.DTO.Common;
using RuyaOptik.DTO.Product;

namespace RuyaOptik.Business.Interfaces
{
    public interface IProductService
    {
        Task<PagedResultDto<ProductDto>> GetPagedAsync(int page, int pageSize);

        Task<PagedResultDto<ProductDto>> GetFilteredPagedAsync(
            int page,
            int pageSize,
            ProductFilterDto filter,
            ProductSortOption sort);

        Task<List<ProductDto>> GetAllAsync();
        Task<List<ProductDto>> GetActiveAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductCreateDto dto);
        Task<bool> UpdateAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
