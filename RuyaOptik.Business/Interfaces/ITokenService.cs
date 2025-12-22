using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using RuyaOptik.DTO.Auth;

namespace RuyaOptik.Business.Interfaces
{
    public interface ITokenService
    {
        public TokenDto CreateAccessToken(int minute);
        public string CreateRefreshToken();
    }
}