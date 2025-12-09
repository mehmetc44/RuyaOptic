using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace RuyaOptik.Entity.Entities
{
    public class AspUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }    

        
    }
}