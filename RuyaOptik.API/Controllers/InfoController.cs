using Microsoft.AspNetCore.Mvc;
using RuyaOptik.DTO.System;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using RuyaOptik.Entity.Enums;
using Microsoft.AspNetCore.Authorization;
namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfoController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public InfoController(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// API about / info endpoint
        [HttpGet]
        [Authorize(Roles ="Admin")]
        [AuthorizeDefinition(Action=ActionType.Reading,Definition = "API Hakkında",Menu=AuthorizeDefinitionConstants.Info)]
        public IActionResult Get()
        {
            var info = new ApiInfoDto
            {
                Application = "RuyaOptik API",
                Version = "1.0.0",
                Environment = _env.EnvironmentName,
                ServerTime = DateTime.UtcNow
            };

            return Ok(info);
        }
    }
}
