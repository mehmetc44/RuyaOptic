using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.Entity.Configurations;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using RuyaOptik.Entity.Enums;
using Microsoft.AspNetCore.Authorization;
using RuyaOptik.DTO.Mail;
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
        [Authorize(Roles ="Admin")]
        [AuthorizeDefinition(Action=ActionType.Reading,Definition = "Adminin Erişebileceği Tanımlar",Menu=AuthorizeDefinitionConstants.Auth)]
        public async Task<IActionResult> GetAuthorizeDefinitions()
        {
            var data = await _appService.GetAuthorizeDefinitionEndpoints(typeof(Program));
            return Ok(data);
        }
        [HttpPost("send-test-mail")]
        [AuthorizeDefinition(Action=ActionType.Reading,Definition = "Test Mail Gönder",Menu=AuthorizeDefinitionConstants.Auth)]
        public async Task<IActionResult> SendTestMail([FromBody] SendMailDto sendMailDto)
        {
            await _mailService.SendMailAsync(sendMailDto.To, sendMailDto.Subject, sendMailDto.Body);
            return Ok();
        }
    }
}
