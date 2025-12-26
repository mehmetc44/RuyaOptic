using RuyaOptik.Entity.Identity;
using RuyaOptik.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RuyaOptik.Business.Interfaces
{
    public interface IUserService
    {
        Task<CreateUserResponseDto> CreateAsync(CreateUserDto model);
        Task UpdateRefreshTokenAsync(string refreshToken, AspUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        Task<List<UserDto>> GetAllUsersAsync(int page, int size);
        Task<int> TotalUsersCount();
        Task AssignRoleToUserAsnyc(string userId, string[] roles);
        Task<List<string>> GetRolesFromUserAsync(string userIdOrName);
        Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
    }
}
