using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Category;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using RuyaOptik.Entity.Enums;
using Microsoft.AspNetCore.Authorization;
namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/category/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (result is null) return NotFound();

            return Ok(result);
        }

        // POST: api/category
        [HttpPost]
        [Authorize("Admin")]
        [AuthorizeDefinition(Action=ActionType.Writing,Definition = "Kategori Oluştur",Menu=AuthorizeDefinitionConstants.Category)]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/category/5
        [HttpPut("{id:int}")]
        [Authorize("Admin")]
        [AuthorizeDefinition(Action=ActionType.Updating,Definition = "Kategori Güncelle",Menu=AuthorizeDefinitionConstants.Category)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _categoryService.UpdateAsync(id, dto);
            if (!success) return NotFound();

            return NoContent();
        }

        // DELETE: api/category/5
        [HttpDelete("{id:int}")]
        [Authorize("Admin")]
        [AuthorizeDefinition(Action=ActionType.Deleting,Definition = "Kategori Sil",Menu=AuthorizeDefinitionConstants.Category)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryService.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
