using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces.Configurations;

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
        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            var Datas = _appService.getAuthorizeDefinitionEndpoints();
            return Ok(Datas);

        }
    }
}
