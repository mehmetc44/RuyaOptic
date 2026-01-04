 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Auth;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Consts;
using RuyaOptik.Entity.Enums;


namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService,IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AspUserLoginDto model)
        {
            try
            {
                var token = await _authService.LoginAsync( new AspUserLoginDto()
                {
                    UserNameOrEmail = model.UserNameOrEmail,
                    Password= model.Password
                },900);
                return Ok(new
                {
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = ex.Message
                });
            }
        }
        [HttpPost("refresh-token")]
        public async Task<TokenDto> RefreshTokenLogin([FromForm] string refreshToken)
        {
            TokenDto token= await _authService.RefreshTokenLoginAsync(refreshToken);
            return token;
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> ResetPassword([FromForm] string email)
        {
             await _authService.PasswordResetAsnyc(email);
             return Ok();
        }

        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromForm] string resetToken, string userId)
        {
            var res = await _authService.VerifyResetTokenAsync(resetToken,userId);
            return Ok(res);
        }
    }
}