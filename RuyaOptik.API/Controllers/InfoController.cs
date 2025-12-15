using Microsoft.AspNetCore.Mvc;
using RuyaOptik.DTO.System;

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
