using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Role;
using RuyaOptik.Entity.Identity;

namespace RuyaOptik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleservice)
        {
            _roleService = roleservice;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var datas = await  _roleService.GetAllRoles();

            return Ok(datas);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleService.GetRoleById(id);

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRole = await _roleService.CreateRole(dto);

            return CreatedAtAction(
                nameof(GetRoleById),       
                new { id = createdRole.Id },
                createdRole                 
            );

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] UpdateRoleDto dto)
        {
            await _roleService.UpdateRole(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string id)
        {
            await _roleService.DeleteRole(id);
            return NoContent();
        }



    }
}
