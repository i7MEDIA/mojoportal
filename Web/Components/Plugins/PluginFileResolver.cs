#nullable enable
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Web.Hosting;

namespace mojoPortal.Web.Plugins;

public static class PluginFileResolver
{
	private static readonly ConcurrentDictionary<string, string> _resolvedPathCache = new(StringComparer.OrdinalIgnoreCase);
	private static readonly ConcurrentDictionary<string, string> _resolvedDirCache = new(StringComparer.OrdinalIgnoreCase);


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

		if (IsExcludedSpecialPath(normalized))
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

		if (string.IsNullOrWhiteSpace(normalized) || IsExcludedSpecialPath(normalized))
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


	public static void ClearCache()
	{
		_resolvedPathCache.Clear();
		_resolvedDirCache.Clear();
	}


	/// <summary>
	/// Resolves a root-relative or app-relative virtual directory path (e.g., "~/Blog/Controls")
	/// to its physical location inside an enabled plugin ("~/Plugins/&lt;FolderName&gt;/&lt;SubPath&gt;") if present.
	/// </summary>
	public static string ResolveVirtualDirectory(string virtualDir)
	{
		if (string.IsNullOrWhiteSpace(virtualDir))
		{
			return virtualDir;
		}

		var normalized = NormalizeAppRelativePath(virtualDir);

		if (string.IsNullOrWhiteSpace(normalized))
		{
			return virtualDir;
		}

		if (IsExcludedSpecialPath(normalized))
		{
			return normalized;
		}

		return _resolvedDirCache.GetOrAdd(normalized, ResolveDirUncached);
	}


	/// <summary>
	/// Attempts to resolve a virtual directory path to a plugin virtual directory ("~/Plugins/&lt;FolderName&gt;/&lt;SubPath&gt;").
	/// </summary>
	public static bool TryResolvePluginDirectory(string virtualDir, out string? pluginVirtualDir)
	{
		pluginVirtualDir = null;

		if (string.IsNullOrWhiteSpace(virtualDir))
		{
			return false;
		}

		var normalized = NormalizeAppRelativePath(virtualDir);

		if (string.IsNullOrWhiteSpace(normalized) || IsExcludedSpecialPath(normalized))
		{
			return false;
		}

		var resolved = ResolveVirtualDirectory(normalized);

		if (!string.Equals(resolved, normalized, StringComparison.OrdinalIgnoreCase) &&
			resolved.StartsWith("~/Plugins/", StringComparison.OrdinalIgnoreCase))
		{
			pluginVirtualDir = resolved;

			return true;
		}

		return false;
	}


	private static string ResolveDirUncached(string normalizedAppRelativeDir)
	{
		var relativeSubPath = normalizedAppRelativeDir.Substring(2).Trim('/').Replace('/', Path.DirectorySeparatorChar);

		if (string.IsNullOrWhiteSpace(relativeSubPath))
		{
			return normalizedAppRelativeDir;
		}

		foreach (var plugin in PluginLoader.EnabledPlugins)
		{
			if (string.IsNullOrWhiteSpace(plugin.PhysicalPath))
			{
				continue;
			}

			var candidatePhysicalDir = Path.Combine(plugin.PhysicalPath, relativeSubPath);

			if (Directory.Exists(candidatePhysicalDir))
			{
				var subUrl = normalizedAppRelativeDir.Substring(2).Trim('/');

				return $"~/Plugins/{plugin.FolderName}/{subUrl}";
			}
		}

		return normalizedAppRelativeDir;
	}


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


	private static bool IsExcludedSpecialPath(string normalized)
	{
		var trimmed = normalized.TrimEnd('/');

		return trimmed.Equals("~/Plugins", StringComparison.OrdinalIgnoreCase) ||
			trimmed.StartsWith("~/Plugins/", StringComparison.OrdinalIgnoreCase) ||
			trimmed.Equals("~/Data", StringComparison.OrdinalIgnoreCase) ||
			trimmed.StartsWith("~/Data/", StringComparison.OrdinalIgnoreCase) ||
			trimmed.Equals("~/App_Themes", StringComparison.OrdinalIgnoreCase) ||
			trimmed.StartsWith("~/App_Themes/", StringComparison.OrdinalIgnoreCase) ||
			trimmed.Equals("~/Setup", StringComparison.OrdinalIgnoreCase) ||
			trimmed.StartsWith("~/Setup/", StringComparison.OrdinalIgnoreCase) ||
			trimmed.Equals("~/App_GlobalResources", StringComparison.OrdinalIgnoreCase) ||
			trimmed.StartsWith("~/App_GlobalResources/", StringComparison.OrdinalIgnoreCase) ||
			trimmed.Equals("~/App_LocalResources", StringComparison.OrdinalIgnoreCase) ||
			trimmed.StartsWith("~/App_LocalResources/", StringComparison.OrdinalIgnoreCase) ||
			trimmed.Equals("~/App_Code", StringComparison.OrdinalIgnoreCase) ||
			trimmed.StartsWith("~/App_Code/", StringComparison.OrdinalIgnoreCase) ||
			trimmed.Equals("~/App_Data", StringComparison.OrdinalIgnoreCase) ||
			trimmed.StartsWith("~/App_Data/", StringComparison.OrdinalIgnoreCase) ||
			trimmed.Equals("~/App_Browsers", StringComparison.OrdinalIgnoreCase) ||
			trimmed.StartsWith("~/App_Browsers/", StringComparison.OrdinalIgnoreCase) ||
			trimmed.Equals("~/bin", StringComparison.OrdinalIgnoreCase) ||
			trimmed.StartsWith("~/bin/", StringComparison.OrdinalIgnoreCase);
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
