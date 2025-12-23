using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.Role;
using RuyaOptik.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuyaOptik.Business.Exceptions;
namespace RuyaOptik.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AspRole> _roleManager;

        public RoleService(RoleManager<AspRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<RoleDto> CreateRole(CreateRoleDto dto)
        {
            var role = new AspRole
            {
                Name = dto.Name.Trim(),
                NormalizedName = dto.Name.Trim().ToUpperInvariant()
            };

            var result = await _roleManager.CreateAsync(role);

            // DEBUG LOG
            Console.WriteLine("Succeeded: " + result.Succeeded);
            foreach (var error in result.Errors)
                Console.WriteLine(error.Code + " - " + error.Description);

            if (!result.Succeeded)
            {
                throw new Exception(
                    result.Errors.Any()
                        ? string.Join(" | ", result.Errors.Select(e => e.Description))
                        : "CreateAsync başarısız ama Identity hata döndürmedi. DB/migration/duplicate role sorunu olabilir."
                );
            }

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

    public async Task UpdateRole(string id, UpdateRoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                throw new NotFoundRoleException();

            role.Name = dto.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new Exception(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
        }

        public async Task DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                throw new NotFoundRoleException();

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                throw new Exception(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
        }


        public async Task<List<RoleDto>> GetAllRoles()
        {
            return await _roleManager.Roles
                .Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name
                })
                .ToListAsync();
        }



        public async Task<RoleDto> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                throw new NotFoundRoleException();

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }


        
    }
}
