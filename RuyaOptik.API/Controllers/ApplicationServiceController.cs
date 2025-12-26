using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.Entity.Configurations;
namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationServiceController : ControllerBase
    {
        readonly IApplicationService _appService;
        public ApplicationServiceController(IApplicationService appService)
        {
            _appService = appService;
        }
        [HttpGet("authorize-definitions")]
        public async Task<IActionResult> GetAuthorizeDefinitions()
        {
            var data = await _appService.GetAuthorizeDefinitionEndpoints(typeof(Program));
            return Ok(data);
        }

    }
}
