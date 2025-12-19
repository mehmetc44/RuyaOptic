using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuyaOptik.DTO.AuthDtos
{
    public class AspUserRegisterDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}