using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.Entity.Configurations;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using RuyaOptik.Entity.Enums;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles ="Admin")]
        [AuthorizeDefinition(Action=ActionType.Reading,Definition = "Adminin Erişebileceği Tanımlar",Menu=AuthorizeDefinitionConstants.Auth)]
        public async Task<IActionResult> GetAuthorizeDefinitions()
        {
            var data = await _appService.GetAuthorizeDefinitionEndpoints(typeof(Program));
            return Ok(data);
        }
    }
}
