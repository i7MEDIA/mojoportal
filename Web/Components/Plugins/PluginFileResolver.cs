#nullable enable
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Web.Hosting;

namespace mojoPortal.Web.Plugins;

public static class PluginFileResolver
{
	private static readonly ConcurrentDictionary<string, string> _resolvedPathCache = new(StringComparer.OrdinalIgnoreCase);


	/// <summary>
	/// Resolves a root-relative or app-relative virtual path (e.g., "~/Blog/EditPost.aspx", "/HtmlEdit.aspx", "Modules/HtmlModule.ascx")
	/// to its physical location inside an enabled plugin ("~/Plugins/&lt;FolderName&gt;/&lt;SubPath&gt;") if present.
	/// Enabled plugins are checked in EffectiveOrder sequence BEFORE root physical files so that upgraded plugins
	/// in ~/Plugins/ always take precedence over leftover root files.
	/// </summary>
	public static string ResolveVirtualPath(string virtualPath)
	{
		if (string.IsNullOrWhiteSpace(virtualPath))
		{
			return virtualPath;
		}

		var normalized = NormalizeAppRelativePath(virtualPath);

		if (string.IsNullOrWhiteSpace(normalized))
		{
			return virtualPath;
		}

		// Already inside ~/Plugins/ or ~/Data/ or ~/App_Themes/ or ~/Setup/
		if (normalized.StartsWith("~/Plugins/", StringComparison.OrdinalIgnoreCase) ||
			normalized.StartsWith("~/Data/", StringComparison.OrdinalIgnoreCase) ||
			normalized.StartsWith("~/App_Themes/", StringComparison.OrdinalIgnoreCase) ||
			normalized.StartsWith("~/Setup/", StringComparison.OrdinalIgnoreCase))
		{
			return normalized;
		}

		return _resolvedPathCache.GetOrAdd(normalized, ResolveUncached);
	}


	/// <summary>
	/// Attempts to resolve a request path to a plugin virtual path ("~/Plugins/&lt;FolderName&gt;/&lt;SubPath&gt;").
	/// Returns true only when the file exists inside an enabled plugin folder.
	/// </summary>
	public static bool TryResolvePluginPath(string virtualPath, out string? pluginVirtualPath)
	{
		pluginVirtualPath = null;

		if (string.IsNullOrWhiteSpace(virtualPath))
		{
			return false;
		}

		var normalized = NormalizeAppRelativePath(virtualPath);

		if (string.IsNullOrWhiteSpace(normalized) ||
			normalized.StartsWith("~/Plugins/", StringComparison.OrdinalIgnoreCase) ||
			normalized.StartsWith("~/Data/", StringComparison.OrdinalIgnoreCase) ||
			normalized.StartsWith("~/App_Themes/", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}

		var resolved = ResolveVirtualPath(normalized);

		if (!string.Equals(resolved, normalized, StringComparison.OrdinalIgnoreCase) &&
			resolved.StartsWith("~/Plugins/", StringComparison.OrdinalIgnoreCase))
		{
			pluginVirtualPath = resolved;

			return true;
		}

		return false;
	}


	/// <summary>
	/// Checks if a virtual path exists either in an enabled plugin folder or at the root physical path.
	/// </summary>
	public static bool FileExists(string virtualPath)
	{
		if (string.IsNullOrWhiteSpace(virtualPath))
		{
			return false;
		}

		var resolved = ResolveVirtualPath(virtualPath);
		var physicalPath = MapPathSafe(resolved);

		return !string.IsNullOrWhiteSpace(physicalPath) && File.Exists(physicalPath);
	}


	public static void ClearCache() => _resolvedPathCache.Clear();


	private static string ResolveUncached(string normalizedAppRelativePath)
	{
		// Strip leading "~/"
		var relativeSubPath = normalizedAppRelativePath.Substring(2).Replace('/', Path.DirectorySeparatorChar);

		if (string.IsNullOrWhiteSpace(relativeSubPath))
		{
			return normalizedAppRelativePath;
		}

		foreach (var plugin in PluginLoader.EnabledPlugins)
		{
			if (string.IsNullOrWhiteSpace(plugin.PhysicalPath))
			{
				continue;
			}

			var candidatePhysicalPath = Path.Combine(plugin.PhysicalPath, relativeSubPath);

			if (File.Exists(candidatePhysicalPath))
			{
				var subUrl = normalizedAppRelativePath.Substring(2);

				return $"~/Plugins/{plugin.FolderName}/{subUrl}";
			}
		}

		return normalizedAppRelativePath;
	}


	private static string NormalizeAppRelativePath(string path)
	{
		var clean = path.Trim();
		int queryIndex = clean.IndexOf('?');

		if (queryIndex >= 0)
		{
			clean = clean.Substring(0, queryIndex);
		}

		if (clean.StartsWith("~/", StringComparison.Ordinal))
		{
			return clean;
		}

		if (clean.StartsWith("~", StringComparison.Ordinal))
		{
			return "~/" + clean.Substring(1).TrimStart('/');
		}

		var appPath = HostingEnvironment.ApplicationVirtualPath ?? "/";

		if (appPath.Length > 1 && clean.StartsWith(appPath + "/", StringComparison.OrdinalIgnoreCase))
		{
			clean = clean.Substring(appPath.Length);
		}

		if (clean.StartsWith("/", StringComparison.Ordinal))
		{
			return "~" + clean;
		}

		return "~/" + clean;
	}


	private static string? MapPathSafe(string virtualPath)
	{
		try
		{
			return HostingEnvironment.MapPath(virtualPath);
		}
		catch
		{
			return null;
		}
	}
}
