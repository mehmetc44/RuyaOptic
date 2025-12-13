using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using RuyaOptik.DTO.AuthDtos;

namespace RuyaOptik.Business.Interfaces
{
    public interface ITokenService
    {
        public AccesTokenDto CreateAccessToken(int minute);
        public void CreateRefreshToken();
    }
}