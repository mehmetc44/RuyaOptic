using AutoMapper;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DTO.Product;
using RuyaOptik.Entity.Entities.Concrete;

namespace RuyaOptik.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetWhereAsync(x => !x.IsDeleted);
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<List<ProductDto>> GetActiveAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.IsDeleted) return null;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
        {
            var entity = _mapper.Map<Product>(dto);

            await _productRepository.AddAsync(entity);
            await _productRepository.SaveChangesAsync();

            return _mapper.Map<ProductDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.IsDeleted) return false;

            _mapper.Map(dto, product);
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.IsDeleted) return false;

            product.IsDeleted = true;
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();

            return true;
        }
    }
}
