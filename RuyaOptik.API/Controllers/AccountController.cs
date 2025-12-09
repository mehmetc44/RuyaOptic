using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RuyaOptik.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

      private UserManager<IdentityUser> _userManager;

      public AccountController(UserManager<IdentityUser> userManager)
      {
        _userManager = userManager;
      }

      public async Task<IActionResult> Register(string email, string password)
      {
        var user = new IdentityUser
        {
          UserName = email,
          Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
          return Ok("User registered successfully.");
        }
        else
        {
          return BadRequest(result.Errors);
        }
      }

    }
}