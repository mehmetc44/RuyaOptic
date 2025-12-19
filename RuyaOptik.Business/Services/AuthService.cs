using Microsoft.AspNetCore.Identity;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.AuthDtos;
using AutoMapper;
using RuyaOptik.Entity.Identity;
using Microsoft.IdentityModel.Tokens;
using RuyaOptik.Business.Exceptions;

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

        public Task<TokenDto> RefreshTokenLoginAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}