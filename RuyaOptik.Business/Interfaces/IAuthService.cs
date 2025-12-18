using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuyaOptik.DTO.AuthDtos;
namespace RuyaOptik.Business.Interfaces
{
    public interface IAuthService
    {
        Task<AspUserRegisterResponseDto> RegisterAsync(AspUserRegisterDto model);
        Task<AspUserLoginResponseDto> LoginAsync(AspUserLoginDto model);
    }
}