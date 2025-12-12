using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Product;

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

        // GET: api/product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/product/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/product/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _productService.UpdateAsync(id, dto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/product/5 (soft delete)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
