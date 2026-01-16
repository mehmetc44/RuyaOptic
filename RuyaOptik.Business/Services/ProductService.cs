using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RuyaOptik.Business.Consts;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DTO.Common;
using RuyaOptik.DTO.Product;
using RuyaOptik.Entity.Concrete;
using System.Linq.Expressions;

namespace RuyaOptik.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly CacheVersionService _version;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper,
            IMemoryCache cache,
            CacheVersionService version)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
            _version = version;
        }

        // FILTER
        private Expression<Func<Product, bool>> BuildFilter(ProductFilterDto filter)
        {
            var search = filter.Search?.ToLower();

            return p =>
                !p.IsDeleted &&
                (!filter.CategoryId.HasValue || p.CategoryId == filter.CategoryId) &&
                (string.IsNullOrWhiteSpace(filter.Brand) || p.Brand == filter.Brand) &&
                (!filter.MinPrice.HasValue || (p.DiscountedPrice ?? p.Price) >= filter.MinPrice) &&
                (!filter.MaxPrice.HasValue || (p.DiscountedPrice ?? p.Price) <= filter.MaxPrice) &&
                (!filter.IsActive.HasValue || p.IsActive == filter.IsActive) &&
                (string.IsNullOrWhiteSpace(search) ||
                    p.Name.ToLower().Contains(search) ||
                    (p.Brand != null && p.Brand.ToLower().Contains(search)) ||
                    (p.ModelCode != null && p.ModelCode.ToLower().Contains(search)) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)));
        }

        // SORTING
        private IQueryable<Product> ApplySorting(IQueryable<Product> query, ProductSortOption sort)
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
                    query.OrderByDescending(p => p.CreatedDate)
            };
        }

        private static string BuildListKey(int page, int pageSize, ProductFilterDto f, ProductSortOption sort)
        {
            var brand = f.Brand?.Trim().ToLower() ?? "";
            var search = f.Search?.Trim().ToLower() ?? "";

            return $"p={page}|ps={pageSize}|c={f.CategoryId?.ToString() ?? ""}|b={brand}|min={f.MinPrice?.ToString() ?? ""}|max={f.MaxPrice?.ToString() ?? ""}|a={f.IsActive?.ToString() ?? ""}|s={search}|sort={sort}";
        }

        // FILTER + SORT + PAGINATION (CACHED + VERSIONED)
        public async Task<PagedResultDto<ProductDto>> GetFilteredPagedAsync(
            int page,
            int pageSize,
            ProductFilterDto filter,
            ProductSortOption sort)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 50 ? 10 : pageSize;

            filter ??= new ProductFilterDto();

            var v = _version.Get(CacheKeys.Product_Version);
            var rawKey = BuildListKey(page, pageSize, filter, sort);
            var cacheKey = CacheKeys.Products_List(v, rawKey);

            if (_cache.TryGetValue(cacheKey, out PagedResultDto<ProductDto> cached))
                return cached;

            var skip = (page - 1) * pageSize;
            var predicate = BuildFilter(filter);

            var query = _productRepository.Query(predicate);
            query = ApplySorting(query, sort);

            var totalCount = await query.CountAsync();
            var products = await query.Skip(skip).Take(pageSize).ToListAsync();

            var result = new PagedResultDto<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(products),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });

            return result;
        }

        public async Task<PagedResultDto<ProductDto>> GetPagedAsync(int page, int pageSize)
        {
            return await GetFilteredPagedAsync(page, pageSize, new ProductFilterDto(), ProductSortOption.Newest);
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

        // GET BY ID (CACHED)
        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var key = CacheKeys.Product_ById(id);

            if (_cache.TryGetValue(key, out ProductDto cached))
                return cached;

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.IsDeleted)
                return null;

            var dto = _mapper.Map<ProductDto>(product);

            _cache.Set(key, dto, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return dto;
        }

        // CRUD (invalidate by version + remove byId)
        public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
        {
            var entity = _mapper.Map<Product>(dto);

            await _productRepository.AddAsync(entity);
            await _productRepository.SaveChangesAsync();

            _cache.Remove(CacheKeys.Product_ById(entity.Id));
            _version.Increment(CacheKeys.Product_Version);

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

            _cache.Remove(CacheKeys.Product_ById(id));
            _version.Increment(CacheKeys.Product_Version);

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

            _cache.Remove(CacheKeys.Product_ById(id));
            _version.Increment(CacheKeys.Product_Version);

            return true;
        }
    }
}
