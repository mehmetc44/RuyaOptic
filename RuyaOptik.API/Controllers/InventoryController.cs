using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Inventory;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using RuyaOptik.Entity.Enums;
using Microsoft.AspNetCore.Authorization;
namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
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
                return NotFound(new { message = "Bu ürün için envanter bulunamadı." });

            return Ok(result);
        }

        // POST: api/inventory
        [HttpPost]
        [AuthorizeDefinition(Action = ActionType.Writing, Definition = "Envanter Oluştur", Menu = AuthorizeDefinitionConstants.Inventory)]
        public async Task<IActionResult> Create([FromBody] InventoryCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _inventoryService.CreateAsync(dto);
            return Ok(new { message = "Envanter başarıyla oluşturuldu." });
        }


        // PUT: api/inventory/5
        [HttpPut("{id:int}")]
        [AuthorizeDefinition(Action = ActionType.Updating, Definition = "Envanter Güncelle", Menu = AuthorizeDefinitionConstants.Inventory)]
        public async Task<IActionResult> Update(int id, [FromBody] InventoryUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _inventoryService.UpdateAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Envanter bulunamadı." });

            return NoContent();
        }
    }
}
