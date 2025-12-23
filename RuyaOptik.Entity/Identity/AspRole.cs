using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace RuyaOptik.Entity.Identity
{
    public class AspRole : IdentityRole
    {
        public string Name { get; set; }

    }
}