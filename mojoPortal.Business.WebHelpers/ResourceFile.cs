using mojoPortal.Web.Framework;
using System.Web;

namespace mojoPortal.Business.WebHelpers;

public static class ResourceFile
{
	public static string GetResourceString(string resourceFile, string resourceKey)
	{
		if (HttpContext.Current == null)
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

			if (resource != null)
			{
				return resource.ToString();
			}
		}
		catch (System.Resources.MissingManifestResourceException)
		{ }

		var pluginResource = ResourceHelper.GetPluginResourceString(resourceFile, resourceKey);

		if (pluginResource != null)
		{
			return pluginResource;
		}

		return resourceKey;
	}
}
