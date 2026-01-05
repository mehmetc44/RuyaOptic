using Microsoft.AspNetCore.Identity;
using RuyaOptik.Entity.Identity;
using RuyaOptik.Entity.Concrete;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.User;
using RuyaOptik.Business.Exceptions;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
namespace RuyaOptik.Business.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AspUser> _userManager;
        readonly RoleManager<AspRole> _roleManager;
        readonly IRepository<Endpoint> _efRepository;

        public UserService(UserManager<AspUser> userManager, RoleManager<AspRole> roleManager, IRepository<Endpoint> efRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _efRepository = efRepository;
        }

        public async Task<CreateUserResponseDto> CreateAsync(CreateUserDto model)
        {
            var user = new AspUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            CreateUserResponseDto response = new() { Succeeded = result.Succeeded };
                if (result.Succeeded)
                {
                    var userRole = await _roleManager.FindByNameAsync("User");
                    if (userRole != null)
                    {
                        if (!await _userManager.IsInRoleAsync(user, userRole.Name))
                        {
                            await _userManager.AddToRoleAsync(user, userRole.Name);
                        }
                    }
                    else
                    {
                        throw new Exception("Varsayılan 'User' rolü veritabanında bulunamadı!");
                    }

                    response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
                }
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";
            return response;
        }

        public async Task<List<UserGetDto>> GetAllUsersAsync(int page, int size)
        {
            var users = await _userManager.Users
                .Skip(page * size)
                .Take(size)
                .Select(u => new UserGetDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.UserName,
                    Email = u.Email,
                }).ToListAsync();

            return users;
        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AspUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                string decodedToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(resetToken)
                );
                IdentityResult result =
                    await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                }
                else
                {
                    throw new PasswordChangeFailedException();
                }
            }
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AspUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();
        }
        public async Task<int> TotalUsersCount()
        {
            return await _userManager.Users.CountAsync();
        }

        public async Task AssignRoleToUserAsnyc(string userId, string[] roles)
        {
            AspUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundUserException();

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
                if (!removeResult.Succeeded)
                    throw new Exception("Existing roles could not be removed");
            }

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    throw new Exception($"Role '{role}' does not exist");
            }

            var addResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addResult.Succeeded)
                throw new Exception("Roles could not be added");
        }


        public async Task<List<string>> GetRolesFromUserAsync(string userIdOrName)
        {
            AspUser? user = await _userManager.FindByIdAsync(userIdOrName) 
                ?? await _userManager.FindByNameAsync(userIdOrName);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }else 
                return new List<string>();

        }

      
        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
         var userRoles = await this.GetRolesFromUserAsync(name);

        if (!userRoles.Any())
            return false;

        Endpoint? endpoint = await _efRepository.Table.Include(e => e.Roles).FirstOrDefaultAsync(e => e.Code == code);

        if (endpoint == null)
            return false;

        var hasRole = false;
        var endpointRoles = endpoint.Roles.Select(r => r.Name);

        foreach (var userRole in userRoles)
        {
            foreach (var endpointRole in endpointRoles)
                if (userRole == endpointRole)
                    return true;
        }

        return false;

        }
    }    
}
