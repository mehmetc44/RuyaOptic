using RuyaOptik.DTO.Common;
using RuyaOptik.DTO.Product;

namespace RuyaOptik.Business.Interfaces
{
    public interface IProductService
    {
        // PAGINATION
        Task<PagedResultDto<ProductDto>> GetPagedAsync(int page, int pageSize);


        Task<List<ProductDto>> GetAllAsync();
        Task<List<ProductDto>> GetActiveAsync();

        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductCreateDto dto);
        Task<bool> UpdateAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
