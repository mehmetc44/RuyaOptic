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
    public class RolePermissionFilter : IAsyncActionFilter
    {

        private readonly IUserService _userService;
        public RolePermissionFilter(UserService userService)
        {
            _userService = userService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity?.Name;
            if (!string.IsNullOrEmpty(name))
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var attribute = descriptor.MethodInfo.GetCustomAttribute(typeof(AuthorizeDefinitionAttribute))
                    as AuthorizeDefinitionAttribute;
                var httpAttribute = descriptor.MethodInfo.GetCustomAttribute(typeof(HttpMethodAttribute))
                    as HttpMethodAttribute;

                var code = $"{(httpAttribute != null ? httpAttribute.HttpMethods.First() : 
                    HttpMethods.Get)}.{attribute.Action}.{attribute.Definition.Replace(" ", "")}";

                var hasRole = await _userService.HasRolePermissionToEndpointAsync(name,code);
                if (!hasRole)
                {
                    context.Result = new UnauthorizedResult();
                }else 
                    await next();
            }else 
                await next();
        }
    }
}
