using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuyaOptik.DTO.AuthDtos
{
    public class TokenDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }
}