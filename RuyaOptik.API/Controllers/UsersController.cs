using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.AuthDtos;
using RuyaOptik.DTO.UserDtos;

namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UsersController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<CreateUserResponse> CreateUserAsync(CreateUser user)
        {
            var result = await _userService.CreateAsync(new CreateUser
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                PasswordConfirm = user.PasswordConfirm,
                Username = user.Username
            });

            return new CreateUserResponse
            {
                Message = result.Message,
                Succeeded = result.Succeeded
            };
        }

 
        
    }
}