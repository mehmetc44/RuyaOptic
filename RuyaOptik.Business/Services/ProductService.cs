using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DTO.Common;
using RuyaOptik.DTO.Product;
using RuyaOptik.Entity.Entities.Concrete;
using System.Linq.Expressions;

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

        // FILTER
        private Expression<Func<Product, bool>> BuildFilter(ProductFilterDto filter)
        {
            return p =>
                !p.IsDeleted &&
                (!filter.CategoryId.HasValue || p.CategoryId == filter.CategoryId) &&
                (string.IsNullOrWhiteSpace(filter.Brand) || p.Brand == filter.Brand) &&
                (!filter.MinPrice.HasValue || (p.DiscountedPrice ?? p.Price) >= filter.MinPrice) &&
                (!filter.MaxPrice.HasValue || (p.DiscountedPrice ?? p.Price) <= filter.MaxPrice) &&
                (!filter.IsActive.HasValue || p.IsActive == filter.IsActive);
        }

        // SORTING
        private IQueryable<Product> ApplySorting(
            IQueryable<Product> query,
            ProductSortOption sort)
        {
            return sort switch
            {
                ProductSortOption.PriceAsc =>
                    query.OrderBy(p => (double)(p.DiscountedPrice ?? p.Price)),

                ProductSortOption.PriceDesc =>
                    query.OrderByDescending(p => (double)(p.DiscountedPrice ?? p.Price)),

                ProductSortOption.Oldest =>
                    query.OrderBy(p => p.CreatedDate),

                _ =>
                    query.OrderByDescending(p => p.CreatedDate) // Newest
            };
        }

        // FILTER + SORT + PAGINATION
        public async Task<PagedResultDto<ProductDto>> GetFilteredPagedAsync(
            int page,
            int pageSize,
            ProductFilterDto filter,
            ProductSortOption sort)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 50 ? 10 : pageSize;

            var predicate = BuildFilter(filter);
            var skip = (page - 1) * pageSize;

            var query = _productRepository.Query(predicate);
            query = ApplySorting(query, sort);

            var totalCount = await query.CountAsync();
            var products = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDto<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(products),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        // PAGINATION ONLY
        public async Task<PagedResultDto<ProductDto>> GetPagedAsync(int page, int pageSize)
        {
            return await GetFilteredPagedAsync(
                page,
                pageSize,
                new ProductFilterDto(),
                ProductSortOption.Newest);
        }

        // CRUD
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
            if (product == null || product.IsDeleted)
                return null;

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
            if (product == null || product.IsDeleted)
                return false;

            _mapper.Map(dto, product);
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.IsDeleted)
                return false;

            product.IsDeleted = true;
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }
    }
}
