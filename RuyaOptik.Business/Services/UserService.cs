using Microsoft.AspNetCore.Identity;
using RuyaOptik.Entity.Identity;
using RuyaOptik.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuyaOptik.DTO.User;
using RuyaOptik.Business.Exceptions;

namespace RuyaOptik.Business.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AspUser> _userManager;

        public UserService(UserManager<AspUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponseDto> CreateAsync(CreateUserDto model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            }, model.Password);

            CreateUserResponseDto response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";
            return response;
        }


        public async Task UpdateRefreshTokenAsync(string refreshToken, AspUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();
        }
    }
}
