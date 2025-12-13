using Microsoft.AspNetCore.Identity;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.AuthDtos;
using AutoMapper;
using RuyaOptik.Entity.Identity;

namespace RuyaOptik.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AspUser> _userManager;
        private readonly SignInManager<AspUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public AuthService(UserManager<AspUser> userManager, SignInManager<AspUser> signInManager, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            this._signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }


        public async Task<AspUserRegisterResponseDto> RegisterAsync(AspUserRegisterDto model)
        {
            var user = _mapper.Map<AspUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new Exception(errors);
            }

            return _mapper.Map<AspUserRegisterResponseDto>(user);
        }
        public async Task<AspUserLoginResponseDto> LoginAsync(AspUserLoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserNameOrEmail)
            ?? await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                throw new Exception("Girdiğiniz şifre hatalı.");
            }
            AccesTokenDto accessToken = _tokenService.CreateAccessToken(10);

            AspUserLoginResponseDto response = new AspUserLoginResponseDto
            {
                AccessToken = accessToken,
                UserId = user.Id,
                UserName = user.UserName,
            };

            return response;
        }

    }
}