using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Auth;
using RuyaOptik.DTO.User;

namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UsersController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
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

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] int page, [FromQuery] int size)
        {
            var users = await _userService.GetAllUsersAsync(page, size);
            return Ok(users);
        }

        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUserAsync(string userId, string[] roles)
        {
            await _userService.AssignRoleToUserAsnyc(userId, roles);
            return Ok();
        }
        [HttpGet("get-roles-from-user/{userIdOrName}")]
        public async Task<IActionResult> GetRolesFromUser([FromRoute] string userIdOrName)
        {
            var roles = await _userService.GetRolesFromUserAsync(userIdOrName);
            return Ok(roles);
        }



    }
}