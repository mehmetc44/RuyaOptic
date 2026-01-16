using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Exceptions;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Auth;
using RuyaOptik.DTO.User;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using RuyaOptik.Entity.Enums;
namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public UsersController(IAuthService authService, IUserService userService, IMailService mailService)
        {
            _authService = authService;
            _userService = userService;
            _mailService = mailService;
        }

        [HttpPost("register")]
        [AuthorizeDefinition(Action=ActionType.Writing,Definition = "Kullanıcı Kaydı Oluştur",Menu=AuthorizeDefinitionConstants.User)]
        public async Task<CreateUserResponseDto> CreateUserAsync(CreateUserDto user)
        {
            var result = await _userService.CreateAsync(new CreateUserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                PasswordConfirm = user.PasswordConfirm,
                Username = user.Username
            });

            return new CreateUserResponseDto
            {
                Message = result.Message,
                Succeeded = result.Succeeded
            };
        }

        [HttpGet("get-users")]
        [Authorize(Roles ="Admin")]
        [AuthorizeDefinition(Action=ActionType.Reading,Definition = "Tüm Kullanıcıları Getir",Menu=AuthorizeDefinitionConstants.User)]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] int page, [FromQuery] int size)
        {
            var users = await _userService.GetAllUsersAsync(page, size);
            return Ok(users);
        }

        [HttpPost("{userId}/roles")]
        [Authorize(Roles ="Admin")]
        [AuthorizeDefinition(Action=ActionType.Updating,Definition = "Kullanıcıya Rol Ata",Menu=AuthorizeDefinitionConstants.User)]
        public async Task<IActionResult> AssignRolesToUser(string userId,[FromBody] string[] roles)
        {
            await _userService.AssignRoleToUserAsnyc(userId, roles);
            return NoContent(); // 204
        }

        [HttpGet("get-roles-from-user/{userIdOrName}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetRolesFromUser([FromRoute] string userIdOrName)
        {
            var roles = await _userService.GetRolesFromUserAsync(userIdOrName);
            return Ok(roles);
        }
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordUpdateDto dto)
        {
            if (!dto.Password.Equals(dto.PasswordConfirm))
                throw new PasswordChangeFailedException("Lütfen şifreyi birebir doğrulayınız.");

            await _userService.UpdatePasswordAsync(dto.UserId, dto.ResetToken, dto.Password);
            return Ok();
        }

    }
}