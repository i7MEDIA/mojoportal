#nullable enable
using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Web;

[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.High)]
public class mojoVirtualPathProvider : VirtualPathProvider
{
	public mojoVirtualPathProvider() : base()
	{ }


	/// <summary>
	///  AppInitialize() is not used in mojoportal. It would be called on startup
	///  if this file was in the App_Code folder 
	/// </summary>	
	public static void AppInitialize() => HostingEnvironment.RegisterVirtualPathProvider(new mojoVirtualPathProvider());


	private bool IsThemePath(string virtualPath) => virtualPath is not null && virtualPath.Contains("App_Themes");


	public override bool FileExists(string virtualPath)
	{
		if (IsThemePath(virtualPath))
		{
			// just pretend it exists
			return true;
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
}
