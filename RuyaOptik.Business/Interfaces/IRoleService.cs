using RuyaOptik.DTO.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuyaOptik.Business.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRoles();
        Task<RoleDto> GetRoleById(string Id);
        Task<RoleDto> CreateRole(CreateRoleDto dto);
        Task DeleteRole(string id);
        Task UpdateRole(string id, UpdateRoleDto dto);

    }
}
