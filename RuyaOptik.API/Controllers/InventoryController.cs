using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Inventory;

namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: api/inventory/product/5
        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var result = await _inventoryService.GetByProductIdAsync(productId);
            if (result == null)
                return NotFound(new { message = "Inventory not found for this product." });

            return Ok(result);
        }

        // POST: api/inventory
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _inventoryService.CreateAsync(dto);
            return Ok(new { message = "Inventory created successfully." });
        }


        // PUT: api/inventory/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] InventoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _inventoryService.UpdateAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Inventory not found." });

            return NoContent();
        }
    }
}
