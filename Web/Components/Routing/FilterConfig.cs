#nullable enable
using log4net;
using mojoPortal.Web.Plugins;
using System;
using System.Web.Mvc;

namespace mojoPortal.Web.Routing;

public class FilterConfig
{
	private static readonly ILog log = LogManager.GetLogger(typeof(FilterConfig));


	public static void RegisterGlobalFilters(GlobalFilterCollection filters)
	{
		filters.Add(new HandleErrorAttribute());

		foreach (var descriptor in PluginLoader.EnabledPlugins)
		{
			if (descriptor.Instance == null)
			{
				continue;
			}

			try
			{
				descriptor.Instance.RegisterGlobalFilters(filters);
			}
			catch (Exception ex)
			{
				log.Error($"Failed to register global filters for plugin '{descriptor.Name}'.", ex);
			}
		}

		try
		{
			var registrarConfig = RoutesConfig.GetConfig();

			foreach (var registrar in registrarConfig.RouteRegistrars)
			{
				try
				{
					registrar.RegisterGlobalFilters(filters);
				}
				catch (Exception ex)
				{
					log.Error(ex);
				}
			}
		}
		catch (Exception ex)
		{
			log.Error(ex);
		}
	}
}
