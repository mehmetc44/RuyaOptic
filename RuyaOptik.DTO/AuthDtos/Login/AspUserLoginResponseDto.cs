using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuyaOptik.DTO.AuthDtos
{
    public class AspUserLoginResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public AccesTokenDto AccessToken { get; set; } = new AccesTokenDto();
    }
}