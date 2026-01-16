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
        readonly IMailService _mailService;
        public ApplicationServiceController(IApplicationService appService, IMailService mailService)
        {
            _appService = appService;
            _mailService = mailService;
        }
        [HttpGet("authorize-definitions")]
        [AuthorizeDefinition(Action=ActionType.Reading,Definition = "Adminin Erişebileceği Tanımlar",Menu=AuthorizeDefinitionConstants.Auth)]
        public async Task<IActionResult> GetAuthorizeDefinitions()
        {
            var data = await _appService.GetAuthorizeDefinitionEndpoints(typeof(Program));
            return Ok(data);
        }
        
        [HttpGet("send-test-mail")]
        [AuthorizeDefinition(Action=ActionType.Reading,Definition = "Test Mail Gönder",Menu=AuthorizeDefinitionConstants.Auth)]
        public async Task<IActionResult> SendTestMail()
        {
            await _mailService.SendMailAsync("cakmakm4400@gmail.com", "Test Subject", "Test Body");
            return Ok();
        }
    }
}
