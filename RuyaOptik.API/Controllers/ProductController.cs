using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Product;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using RuyaOptik.Entity.Enums;
using Microsoft.AspNetCore.Authorization;
namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // FILTER + SEARCH + SORT + PAGINATION
        // GET: api/product?page=1&pageSize=10&search=ray&sort=PriceAsc
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] ProductFilterDto? filter = null,
            [FromQuery] ProductSortOption sort = ProductSortOption.Newest)
        {
            filter ??= new ProductFilterDto();

            var result = await _productService
                .GetFilteredPagedAsync(page, pageSize, filter, sort);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize("Admin")]
        [AuthorizeDefinition(Action=ActionType.Writing,Definition = "Ürün Oluştur",Menu=AuthorizeDefinitionConstants.Product)]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var created = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        [Authorize("Admin")]
        [AuthorizeDefinition(Action=ActionType.Updating,Definition = "Ürün Güncelle",Menu=AuthorizeDefinitionConstants.Product)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            var success = await _productService.UpdateAsync(id, dto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize("Admin")]
        [AuthorizeDefinition(Action=ActionType.Deleting,Definition = "Ürün Sil",Menu=AuthorizeDefinitionConstants.Product)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
