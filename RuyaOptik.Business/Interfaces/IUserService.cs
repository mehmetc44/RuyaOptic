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
        //Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        //Task<List<ListUser>> GetAllUsersAsync(int page, int size);
        //int TotalUsersCount { get; }
        //Task AssignRoleToUserAsnyc(string userId, string[] roles);
        //Task<string[]> GetRolesToUserAsync(string userIdOrName);
        //Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
    }
}
