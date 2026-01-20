using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.Business.Services;
using System.Reflection;

namespace RuyaOptik.API.Filters
{
    public class RolePermissionFilter : IAsyncAuthorizationFilter
    {

        private readonly IUserService _userService;
        public RolePermissionFilter(UserService userService)
        {
            _userService = userService;
        }
         public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;


        if (user?.Identity?.IsAuthenticated != true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var name = user.Identity!.Name;

        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (descriptor == null) return;

        var attribute = descriptor.MethodInfo
            .GetCustomAttribute<AuthorizeDefinitionAttribute>();

        if (attribute == null) return;

        var httpAttribute = descriptor.MethodInfo
            .GetCustomAttribute<HttpMethodAttribute>();

        var code =
            $"{(httpAttribute?.HttpMethods.First() ?? HttpMethods.Get)}." +
            $"{attribute.Action}." +
            $"{attribute.Definition.Replace(" ", "")}";

        var hasRole =
            await _userService.HasRolePermissionToEndpointAsync(name!, code);

        if (!hasRole)
        {
            context.Result = new ForbidResult();
        }
    }
}
    }

