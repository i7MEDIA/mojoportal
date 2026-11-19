#nullable enable
using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Web;

[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.High)]
public class mojoVirtualPathProvider : VirtualPathProvider
{
	public mojoVirtualPathProvider() : base()
	{ }


	/// <summary>
	/// Registers mojoVirtualPathProvider if it has not already been registered.
	/// </summary>	
	public static void AppInitialize()
	{
		if (HostingEnvironment.VirtualPathProvider is not mojoVirtualPathProvider)
		{
			HostingEnvironment.RegisterVirtualPathProvider(new mojoVirtualPathProvider());
		}
	}


	private bool IsThemePath(string virtualPath) => virtualPath is not null && virtualPath.Contains("App_Themes");


	private static bool IsGlobalResourcesDirectory(string? virtualDir)
	{
		if (string.IsNullOrWhiteSpace(virtualDir))
		{
			return false;
		}

		try
		{
			var appRelative = VirtualPathUtility.ToAppRelative(virtualDir!).TrimEnd('/');

			return string.Equals(appRelative, "~/App_GlobalResources", StringComparison.OrdinalIgnoreCase);
		}
		catch
		{
			return virtualDir!.IndexOf("App_GlobalResources", StringComparison.OrdinalIgnoreCase) >= 0 &&
				virtualDir.IndexOf("Plugins", StringComparison.OrdinalIgnoreCase) < 0;
		}
	}


	internal static bool IsFilteredGlobalResourceFile(string? virtualPath)
	{
		if (string.IsNullOrWhiteSpace(virtualPath))
		{
			return false;
		}

		try
		{
			var appRelative = VirtualPathUtility.ToAppRelative(virtualPath!);

			if (!appRelative.StartsWith("~/App_GlobalResources/", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
		}
		catch
		{
			if (virtualPath!.IndexOf("App_GlobalResources", StringComparison.OrdinalIgnoreCase) < 0 ||
				virtualPath.IndexOf("Plugins", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				return false;
			}
		}

		var fileName = Path.GetFileNameWithoutExtension(virtualPath);

		if (string.IsNullOrWhiteSpace(fileName))
		{
			return false;
		}

		int dotIndex = fileName.IndexOf('.');
		var baseClassName = dotIndex > 0 ? fileName.Substring(0, dotIndex) : fileName;

		return ResourceHelper.IsPluginResource(baseClassName);
	}


	public override bool FileExists(string virtualPath)
	{
		if (IsThemePath(virtualPath))
		{
			// just pretend it exists
			return true;
		}

		if (IsFilteredGlobalResourceFile(virtualPath))
		{
			return false;
		}

		if (PluginFileResolver.TryResolvePluginPath(virtualPath, out var pluginVirtualPath))
		{
			return Previous.FileExists(pluginVirtualPath);
		}

		return Previous.FileExists(virtualPath);
	}


	public override bool DirectoryExists(string virtualDir)
	{
		if (IsThemePath(virtualDir))
		{
			return !virtualDir.Contains("Resources");
		}

		if (PluginFileResolver.TryResolvePluginDirectory(virtualDir, out var pluginVirtualDir))
		{
			return Previous.DirectoryExists(pluginVirtualDir);
		}

		return Previous.DirectoryExists(virtualDir);
	}


	public override string CombineVirtualPaths(string basePath, string relativePath) => base.CombineVirtualPaths(basePath, relativePath);


	public override VirtualDirectory GetDirectory(string virtualDir)
	{
		if (IsThemePath(virtualDir))
		{
			return new mojoThemeVirtualDirectory(Previous.GetDirectory(virtualDir));
		}

		if (IsGlobalResourcesDirectory(virtualDir))
		{
			return new mojoGlobalResourcesVirtualDirectory(Previous.GetDirectory(virtualDir));
		}

		return Previous.GetDirectory(virtualDir);
	}


	public override VirtualFile? GetFile(string virtualPath)
	{
		if (virtualPath is null)
		{
			return null;
		}

		var loweredPath = virtualPath.ToLower();

		if (loweredPath.EndsWith(".skin"))
		{
			try
			{
				return new mojoThemeVirtualFile(virtualPath);
			}
			catch (UnauthorizedAccessException) { }

		}

		if (PluginFileResolver.TryResolvePluginPath(virtualPath, out var pluginVirtualPath))
		{
			var physicalPath = HostingEnvironment.MapPath(pluginVirtualPath);

			if (!string.IsNullOrWhiteSpace(physicalPath) && File.Exists(physicalPath))
			{
				return new mojoPluginVirtualFile(virtualPath, physicalPath);
			}
		}

		return Previous.GetFile(virtualPath);
	}

	public override CacheDependency GetCacheDependency(
		string virtualPath,
		IEnumerable virtualPathDependencies,
		DateTime utcStart)
	{
		if (IsThemePath(virtualPath))
		{
			var dependency = new AggregateCacheDependency();
			var pathToWebConfig = CacheHelper.GetPathToWebConfigFile();

			dependency.Add(new CacheDependency(pathToWebConfig));

			if (WebConfigSettings.UseCacheDependencyFiles)
			{
				var pathToDependencyFile = CacheHelper.GetPathToThemeCacheDependencyFile();

				if (!string.IsNullOrEmpty(pathToDependencyFile))
				{
					dependency.Add(new CacheDependency(pathToDependencyFile));
				}
			}

			var pathToThemeFile = SiteUtils.GetFullPathToThemeFile();

			if (!string.IsNullOrEmpty(pathToThemeFile))
			{
				try
				{
					dependency.Add(new CacheDependency(pathToThemeFile));
				}
				catch (HttpException)
				{
					// this can happen if the site is configured for a skin that doesn't exist
				}
			}

			return dependency;
		}

		var resolvedVirtualPath = PluginFileResolver.ResolveVirtualPath(virtualPath);

		if (virtualPathDependencies is not null)
		{
			var resolvedDependencies = new System.Collections.Generic.List<string>();

			foreach (var dep in virtualPathDependencies)
			{
				if (dep is string depPath && !string.IsNullOrWhiteSpace(depPath))
				{
					resolvedDependencies.Add(PluginFileResolver.ResolveVirtualPath(depPath));
				}
			}

			return Previous.GetCacheDependency(resolvedVirtualPath, resolvedDependencies, utcStart);
		}

		return Previous.GetCacheDependency(resolvedVirtualPath, virtualPathDependencies, utcStart);
	}


	private sealed class mojoPluginVirtualFile : VirtualFile
	{
		private readonly string _physicalPath;

		public mojoPluginVirtualFile(string virtualPath, string physicalPath) : base(virtualPath)
		{
			_physicalPath = physicalPath;
		}

		public override Stream Open()
		{
			return new FileStream(
				_physicalPath,
				FileMode.Open,
				FileAccess.Read,
				FileShare.ReadWrite);
		}
	}


	private sealed class mojoGlobalResourcesVirtualDirectory : VirtualDirectory
	{
		private readonly ArrayList _children = [];
		private readonly ArrayList _directories = [];
		private readonly ArrayList _files = [];

		public override IEnumerable Children => _children;
		public override IEnumerable Directories => _directories;
		public override IEnumerable Files => _files;

		public mojoGlobalResourcesVirtualDirectory(VirtualDirectory requestedDirectory)
			: base(requestedDirectory.VirtualPath)
		{
			foreach (VirtualFileBase child in requestedDirectory.Children)
			{
				if (child.IsDirectory)
				{
					_directories.Add(child);
					_children.Add(child);
				}
				else if (!IsFilteredGlobalResourceFile(child.VirtualPath))
				{
					_files.Add(child);
					_children.Add(child);
				}
			}
		}
	}
}
