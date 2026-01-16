using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using RuyaOptik.Business.Attributes;
using RuyaOptik.Business.Interfaces;
using RuyaOptik.Entity.Configurations;
using RuyaOptik.Entity.Enums;

namespace RuyaOptik.Business.Services
{
	public class ApplicationService : IApplicationService
	{
		private static readonly object _lock = new();
		private static List<Menu>? _cachedMenus;

		public Task<List<Menu>> GetAuthorizeDefinitionEndpoints(Type type)
		{
			if (_cachedMenus != null)
				return Task.FromResult(_cachedMenus);

			lock (_lock)
			{
				if (_cachedMenus != null)
					return Task.FromResult(_cachedMenus);

				Assembly assembly = Assembly.GetAssembly(type)!;
				var controllers = assembly.GetTypes()
					.Where(t => t.IsAssignableTo(typeof(ControllerBase)));

				List<Menu> menus = new();

				foreach (var controller in controllers)
				{
					var actions = controller.GetMethods()
						.Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute), false));

					foreach (var action in actions)
					{
						var attributes = action.GetCustomAttributes(true);

						var authorizeDefinitionAttribute =
							attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute))
							as AuthorizeDefinitionAttribute;

						if (authorizeDefinitionAttribute == null)
							continue;

						Menu? menu;
						if (!menus.Any(m => m.Name == authorizeDefinitionAttribute.Menu))
						{
							menu = new() { Name = authorizeDefinitionAttribute.Menu };
							menus.Add(menu);
						}
						else
						{
							menu = menus.First(m => m.Name == authorizeDefinitionAttribute.Menu);
						}

						Entity.Configurations.Action _action = new()
						{
							ActionType = authorizeDefinitionAttribute.Action,
							Definition = authorizeDefinitionAttribute.Definition
						};

						var httpAttribute = attributes
							.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute)))
							as HttpMethodAttribute;

						_action.HttpType = httpAttribute != null
							? httpAttribute.HttpMethods.First()
							: HttpMethods.Get;

						_action.Code = $"{_action.HttpType}.{_action.ActionType}.{_action.Definition.Replace(" ", "")}";

						menu.Actions.Add(_action);
					}
				}

				_cachedMenus = menus;
				return Task.FromResult(_cachedMenus);
			}
		}
	}
}
