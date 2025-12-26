using Microsoft.AspNetCore.Identity;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Auth;
using AutoMapper;
using RuyaOptik.Entity.Identity;
using Microsoft.IdentityModel.Tokens;
using RuyaOptik.Business.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;


namespace RuyaOptik.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AspUser> _userManager;
        private readonly SignInManager<AspUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        public AuthService(UserManager<AspUser> userManager, SignInManager<AspUser> signInManager, IMapper mapper, ITokenService tokenService, IUserService userService)
        {
            _userManager = userManager;
            this._signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<TokenDto> LoginAsync(AspUserLoginDto model, int accessTokenLifeTime)
        {
            AspUser user = await _userManager.FindByNameAsync(model.UserNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            if (user == null)
                throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded) //Authentication başarılı!
            {
                TokenDto token = _tokenService.CreateAccessToken(accessTokenLifeTime);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 15);
                return token;
            }
            throw new AuthenticationErrorException();
        }

        public async Task<TokenDto> RefreshTokenLoginAsync(string refreshToken)
        {
            AspUser user = _userManager.Users.FirstOrDefault(b => b.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate>DateTime.UtcNow)
            {
                TokenDto token = _tokenService.CreateAccessToken(15);
                _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 10);
                return token;
            }
            else
            throw new NotFoundUserException();
        }
        public async Task PasswordResetAsync(string email)
        {
            AspUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            { 
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));
                await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
            }
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            // Kullanıcıyı ID ile bul
            AspUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Token'ı Base64UrlDecode ile çöz
                byte[] decodedBytes = WebEncoders.Base64UrlDecode(resetToken);
                string decodedToken = Encoding.UTF8.GetString(decodedBytes);

                // Token doğrulama
                return await _userManager.VerifyUserTokenAsync(
                    user,
                    _userManager.Options.Tokens.PasswordResetTokenProvider,
                    "ResetPassword",
                    decodedToken
                );
            }

            return false;
        }
    }
}