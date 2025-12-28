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
using RuyaOptik.DTO.Role;
namespace RuyaOptik.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AspRole> _roleManager;

        public RoleService(RoleManager<AspRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<CreateRoleResponseDto> CreateRole(CreateRoleDto dto)
        {
            AspRole role = new AspRole() { Id = Guid.NewGuid().ToString(), Name = dto.Name, NormalizedName = dto.Name.Trim() };

            IdentityResult result = await _roleManager.CreateAsync(role);


            return new CreateRoleResponseDto
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


        public Task<List<RoleDto>> GetAllRoles()
        {
            return Task.FromResult(_roleManager.Roles.Select(role => new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            }).ToList());
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
