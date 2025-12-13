using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.AuthDtos;
using System.IdentityModel.Tokens.Jwt;
namespace RuyaOptik.Business.Services
{
    public class TokenService : ITokenService
    {
        IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public AccesTokenDto CreateAccessToken(int minute)
        {
            AccesTokenDto token = new AccesTokenDto();

            //Symmetric of our security key
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            
            //Hashing operation
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            token.Expiration = DateTime.Now.AddMinutes(minute);

            //Identifying the token
            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: token.Expiration,
                signingCredentials: credentials
            );
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(jwtToken);
            return token;
        }

        public void CreateRefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}