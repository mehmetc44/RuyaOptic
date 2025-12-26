using Microsoft.AspNetCore.Identity;
using RuyaOptik.Entity.Identity;
using RuyaOptik.Entity.Concrete;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.DTO.User;
using RuyaOptik.Business.Exceptions;
using Microsoft.EntityFrameworkCore;
using RuyaOptik.Entity.Configurations;
using RuyaOptik.DataAccess.Repositories.Interfaces;
using RuyaOptik.DataAccess.Repositories.Concrete;


namespace RuyaOptik.Business.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AspUser> _userManager;
        readonly IRepository<AspRole> _efRepository;

        public UserService(UserManager<AspUser> userManager, EfRepository<AspRole> efRepository)
        {
            _userManager = userManager;
            _efRepository = efRepository;
        }

        public async Task<CreateUserResponseDto> CreateAsync(CreateUserDto model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            }, model.Password);

            CreateUserResponseDto response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";
            return response;
        }

        public async Task<List<UserDto>> GetAllUsersAsync(int page, int size)
        {
            var users = await _userManager.Users
                .Skip(page * size)
                .Take(size)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.UserName,
                    Email = u.Email
                }).ToListAsync();

            return users;
        }

        public Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            throw new NotImplementedException();
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
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.AddToRolesAsync(user, userRoles);
            }else 
                throw new NotFoundUserException();
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

        Endpoint? endpoint = await _efRepository
                    .Include(e => e.Roles)
                    .FirstOrDefaultAsync(e => e.Code == code);

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
