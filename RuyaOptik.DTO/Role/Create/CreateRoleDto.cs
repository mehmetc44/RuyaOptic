using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuyaOptik.DTO.Role
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role name boş olamaz.")]
        [MinLength(3, ErrorMessage = "Role name en az 3 karakter olmalı.")]
        public string Name { get; set; }
    }
}
