using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace RuyaOptik.Entity.Identity
{
    public class AspUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }   = string.Empty;

        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }

        
    }
}