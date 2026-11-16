#nullable enable
using mojoPortal.Web.Framework;
using System.Web;

namespace mojoPortal.Business.WebHelpers;

public static class ResourceFile
{
	public static string GetResourceString(string resourceFile, string resourceKey)
	{
		if (HttpContext.Current is null)
		{
			return ResourceHelper.GetPluginResourceString(resourceFile, resourceKey) ?? resourceKey;
		}

		if (resourceFile.Length == 0)
		{
			resourceFile = "Resource";
		}

		try
		{
			var resource = HttpContext.GetGlobalResourceObject(resourceFile, resourceKey);

			if (resource is not null)
			{
				return resource.ToString();
			}
		}
		catch (System.Resources.MissingManifestResourceException)
		{ }

		var pluginResource = ResourceHelper.GetPluginResourceString(resourceFile, resourceKey);

		if (pluginResource is not null)
		{
			return pluginResource;
		}

		return resourceKey;
	}
}
