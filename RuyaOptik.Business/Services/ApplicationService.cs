using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuyaOptik.Entity.Configurations;
using RuyaOptik.Business.Interfaces;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using RuyaOptik.Business.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using RuyaOptik.Entity.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace RuyaOptik.Business.Services
{
    public class ApplicationService : IApplicationService
	{
		public Task<List<Menu>> GetAuthorizeDefinitionEndpoints(Type type)
		{
			Assembly assembly = Assembly.GetAssembly(type);
			var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

			List<Menu> menus = new();
			if (controllers != null)
				foreach (var controller in controllers)
				{
					var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));
					if (actions != null)
						foreach (var action in actions)
						{
							var attributes = action.GetCustomAttributes(true);
							if (attributes != null)
							{
								Menu menu = null;

								var authorizeDefinitionAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute)) as AuthorizeDefinitionAttribute;
								if (!menus.Any(m => m.Name == authorizeDefinitionAttribute.Menu))
								{
									menu = new() { Name = authorizeDefinitionAttribute.Menu };
									menus.Add(menu);
								}
								else
									menu = menus.FirstOrDefault(m => m.Name == authorizeDefinitionAttribute.Menu);

								DTO.Configurations.Action _action = new()
								{
									ActionType = authorizeDefinitionAttribute.Action,
									Definition = authorizeDefinitionAttribute.Definition
								};

								var httpAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
								if (httpAttribute != null)
									_action.HttpType = httpAttribute.HttpMethods.First();
								else
									_action.HttpType = HttpMethods.Get;

								_action.Code = $"{_action.HttpType}.{_action.ActionType}.{_action.Definition.Replace(" ", "")}";

								menu.Actions.Add(_action);
							}
						}
				}
			return Task.FromResult(menus);
		}
	}
}
