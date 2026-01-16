using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Configuration;
using RuyaOptik.DataAccess.Configurations;

using Microsoft.IdentityModel.Tokens;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
namespace RuyaOptik.Business.Services
{
    public class TokenService : ITokenService
    {
        IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenDto CreateAccessToken(int minute, List<Claim> claims)
        {
            TokenDto token = new TokenDto();
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.jwtKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            token.Expiration = DateTime.Now.AddMinutes(minute);
            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: AppConfiguration.issuer,
                audience: AppConfiguration.audience,
                expires: token.Expiration,
                signingCredentials: credentials,
                claims: claims
            );
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(jwtToken);
            token.RefreshToken = CreateRefreshToken();
            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] data = new byte[32];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
            return Convert.ToBase64String(data);
        }
 

    }
}