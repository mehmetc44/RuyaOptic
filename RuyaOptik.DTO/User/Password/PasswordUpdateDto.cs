using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuyaOptik.DTO.User
{
    public class PasswordUpdateDto
    {
        public string UserId { get; set; }
        public string ResetToken { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
