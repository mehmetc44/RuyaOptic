using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuyaOptik.DTO.AuthDtos;
using RuyaOptik.Entity.Identity;
namespace RuyaOptik.Business.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(AspUserLoginDto model,int accessTokenLifeTime);
        Task<TokenDto> RefreshTokenLoginAsync(string refreshToken, AspUser user, DateTime accessTokenDate, int refrashTokenLifeTime);

        //Task PasswordResetAsnyc(string email);
        //Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
    }
}