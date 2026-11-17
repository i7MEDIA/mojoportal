#nullable enable
using log4net;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;

[assembly: PreApplicationStartMethod(typeof(PluginLoader), nameof(PluginLoader.Initialize))]
namespace mojoPortal.Web.Plugins;

public static class PluginLoader
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(PluginLoader));
	private static readonly object _syncRoot = new();
	private static bool _preAppStartCompleted = false;
	private static bool _pluginsInitialized = false;

	private static readonly List<PluginDescriptor> _allPlugins = [];
	private static readonly List<PluginDescriptor> _enabledPlugins = [];
	private static readonly Dictionary<string, Assembly> _loadedAssembliesBySimpleName = new(StringComparer.OrdinalIgnoreCase);

	public static IReadOnlyList<PluginDescriptor> AllPlugins => _allPlugins;
	public static IReadOnlyList<PluginDescriptor> EnabledPlugins => _enabledPlugins;
	public static IReadOnlyList<PluginDescriptor> LoadedPlugins => _enabledPlugins;


	/// <summary>
	/// Runs during ASP.NET PreApplicationStart before Web Forms / Razor compilation.
	/// Shadow-copies plugin DLLs from ~/Plugins/&lt;FolderName&gt;/bin/ to ~/Data/PluginCache/&lt;FolderName&gt;/
	/// and registers them with BuildManager.AddReferencedAssembly.
	/// </summary>
	public static void Initialize()
	{
		lock (_syncRoot)
		{
			if (_preAppStartCompleted)
			{
				return;
			}

			_preAppStartCompleted = true;

			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

			try
			{
				DiscoverAndLoadPlugins();
			}
			catch (Exception ex)
			{
				_log.Error("PluginLoader failed during PreApplicationStart initialization.", ex);
			}
		}
	}


	/// <summary>
	/// Called during Global.Application_Start to invoke IPlugin.Initialize() on all enabled plugins in Order sequence.
	/// </summary>
	public static void EnsureInitialized()
	{
		if (!_preAppStartCompleted)
		{
			Initialize();
		}

		lock (_syncRoot)
		{
			if (_pluginsInitialized)
			{
				return;
			}

			_pluginsInitialized = true;

			foreach (var descriptor in _enabledPlugins)
			{
				try
				{
					descriptor.Instance?.Initialize();
				}
				catch (Exception ex)
				{
					_log.Error($"Failed to initialize plugin '{descriptor.Name}' ({descriptor.FolderName}).", ex);
				}
			}
		}
	}


	private static void DiscoverAndLoadPlugins()
	{
		var pluginsRoot = HostingEnvironment.MapPath("~/Plugins");
		var binRoot = HostingEnvironment.MapPath("~/bin");
		var cacheRoot = HostingEnvironment.MapPath("~/Data/PluginCache");

		if (string.IsNullOrWhiteSpace(pluginsRoot) || !Directory.Exists(pluginsRoot))
		{
			return;
		}

		// Record DLLs already present in ~/bin/ so we never load a duplicate assembly if ~/bin/ is read-only
		var binDllNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		if (!string.IsNullOrWhiteSpace(binRoot) && Directory.Exists(binRoot))
		{
			foreach (var dllPath in Directory.GetFiles(binRoot, "*.dll", SearchOption.TopDirectoryOnly))
			{
				var simpleName = Path.GetFileNameWithoutExtension(dllPath);

				binDllNames[simpleName] = dllPath;
			}
		}

		var pluginConfig = PluginConfig.Load(pluginsRoot);
		var discoveredDescriptors = new List<PluginDescriptor>();

		foreach (var pluginDir in Directory.GetDirectories(pluginsRoot))
		{
			var folderName = Path.GetFileName(pluginDir);

			if (string.IsNullOrWhiteSpace(folderName) || folderName.StartsWith("."))
			{
				continue;
			}

			var descriptor = new PluginDescriptor
			{
				FolderName = folderName,
				PhysicalPath = pluginDir,
				Enabled = true,
				EffectiveOrder = 100
			};

			if (pluginConfig.Overrides.TryGetValue(folderName, out var folderOverride))
			{
				if (folderOverride.Enabled.HasValue)
				{
					descriptor.Enabled = folderOverride.Enabled.Value;
				}
				if (folderOverride.Order.HasValue)
				{
					descriptor.EffectiveOrder = folderOverride.Order.Value;
				}
			}

			if (!descriptor.Enabled)
			{
				_allPlugins.Add(descriptor);
				continue;
			}

			ResourceHelper.RegisterPluginDirectory(pluginDir);

			var pluginBinDir = Path.Combine(pluginDir, "bin");

			if (Directory.Exists(pluginBinDir))
			{
				var pluginCacheDir = !string.IsNullOrWhiteSpace(cacheRoot)
					? Path.Combine(cacheRoot, folderName)
					: pluginBinDir;

				var canUseCacheDir = TryEnsureDirectory(pluginCacheDir);
				var loadSourceDir = canUseCacheDir ? pluginCacheDir : pluginBinDir;

				foreach (var dllPath in Directory.GetFiles(pluginBinDir, "*.dll", SearchOption.TopDirectoryOnly))
				{
					var simpleName = Path.GetFileNameWithoutExtension(dllPath);

					// If ~/bin/ already contains an assembly with the exact same name:
					// Check if it is a [TypeForwardedTo] compatibility shim or an actual duplicate plugin DLL.
					if (binDllNames.TryGetValue(simpleName, out var existingBinDll))
					{
						_log.Warn($"Plugin '{folderName}' contains '{simpleName}.dll' which also exists in ~/bin/. Using the assembly from ~/bin/ to avoid duplicate type errors on read-only ~/bin/.");

						try
						{
							var binAsm = Assembly.LoadFrom(existingBinDll);

							descriptor.Assemblies.Add(binAsm);

							_loadedAssembliesBySimpleName[simpleName] = binAsm;

							RegisterAssemblyResourceManagers(binAsm);
						}
						catch (Exception ex)
						{
							_log.Error($"Failed to load existing ~/bin/ assembly '{existingBinDll}' for plugin '{folderName}'.", ex);
						}
						continue;
					}

					var targetDllPath = dllPath;

					if (canUseCacheDir)
					{
						var cachedDllPath = Path.Combine(pluginCacheDir, Path.GetFileName(dllPath));

						CopyFileIfChanged(dllPath, cachedDllPath);

						var pdbPath = Path.ChangeExtension(dllPath, ".pdb");

						if (File.Exists(pdbPath))
						{
							var cachedPdbPath = Path.Combine(pluginCacheDir, Path.GetFileName(pdbPath));

							CopyFileIfChanged(pdbPath, cachedPdbPath);
						}

						targetDllPath = File.Exists(cachedDllPath) ? cachedDllPath : dllPath;
					}

					try
					{
						var asm = Assembly.LoadFrom(targetDllPath);

						BuildManager.AddReferencedAssembly(asm);

						descriptor.Assemblies.Add(asm);

						_loadedAssembliesBySimpleName[simpleName] = asm;

						RegisterAssemblyResourceManagers(asm);
					}
					catch (Exception ex)
					{
						_log.Error($"Failed to load plugin assembly '{targetDllPath}' for plugin '{folderName}'.", ex);
					}
				}
			}

			// Find IPlugin implementation in descriptor.Assemblies
			IPlugin? pluginInstance = null;

			foreach (var asm in descriptor.Assemblies)
			{
				foreach (var type in GetLoadableTypes(asm))
				{
					if (!type.IsAbstract && !type.IsInterface && typeof(IPlugin).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) is not null)
					{
						try
						{
							pluginInstance = (IPlugin)Activator.CreateInstance(type);

							break;
						}
						catch (Exception ex)
						{
							_log.Error($"Failed to instantiate IPlugin '{type.FullName}' in plugin '{folderName}'.", ex);
						}
					}
				}

				if (pluginInstance is not null)
				{
					break;
				}
			}

			descriptor.Instance = pluginInstance;

			if (pluginInstance is not null)
			{
				descriptor.EffectiveOrder = pluginInstance.Order;

				// Re-apply config overrides by either FolderName or IPlugin.Name
				if (pluginConfig.Overrides.TryGetValue(folderName, out var fOverride))
				{
					if (fOverride.Order.HasValue)
					{
						descriptor.EffectiveOrder = fOverride.Order.Value;
					}

					if (fOverride.Enabled.HasValue)
					{
						descriptor.Enabled = fOverride.Enabled.Value;
					}
				}

				if (!string.Equals(pluginInstance.Name, folderName, StringComparison.OrdinalIgnoreCase) &&
					pluginConfig.Overrides.TryGetValue(pluginInstance.Name, out var nameOverride))
				{
					if (nameOverride.Order.HasValue)
					{
						descriptor.EffectiveOrder = nameOverride.Order.Value;
					}

					if (nameOverride.Enabled.HasValue)
					{
						descriptor.Enabled = nameOverride.Enabled.Value;
					}
				}
			}

			discoveredDescriptors.Add(descriptor);
		}

		// Sort by EffectiveOrder ascending, then Name ascending
		var sorted = discoveredDescriptors
			.OrderBy(d => d.EffectiveOrder)
			.ThenBy(d => d.Name, StringComparer.OrdinalIgnoreCase)
			.ToList();

		_allPlugins.Clear();
		_allPlugins.AddRange(sorted);

		_enabledPlugins.Clear();
		_enabledPlugins.AddRange(sorted.Where(d => d.Enabled));
	}


	private static void RegisterAssemblyResourceManagers(Assembly assembly)
	{
		var embeddedNames = new HashSet<string>(assembly.GetManifestResourceNames(), StringComparer.OrdinalIgnoreCase);

		foreach (var type in GetLoadableTypes(assembly))
		{
			if (!type.IsClass)
			{
				continue;
			}

			// Skip linked core global resource proxy classes (e.g. Resource, AccessKeys) that are not embedded in this plugin assembly
			if (string.Equals(type.Namespace, "Resources", StringComparison.Ordinal) &&
				!embeddedNames.Contains($"Resources.{type.Name}.resources") &&
				!embeddedNames.Contains($"{type.Name}.resources"))
			{
				continue;
			}

			var rmProp = type.GetProperty("ResourceManager", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

			if (rmProp is not null && typeof(ResourceManager).IsAssignableFrom(rmProp.PropertyType))
			{
				try
				{
					if (rmProp.GetValue(null, null) is ResourceManager rm)
					{
						ResourceHelper.RegisterPluginResourceManager(type.Name, rm);
					}
				}
				catch { }
			}
		}
	}


	private static bool TryEnsureDirectory(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}

		try
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			return true;
		}
		catch (Exception ex)
		{
			_log.Warn($"Could not create plugin cache directory '{path}'. Falling back to direct load.", ex);

			return false;
		}
	}


	private static void CopyFileIfChanged(string sourcePath, string destPath)
	{
		try
		{
			var srcInfo = new FileInfo(sourcePath);
			var dstInfo = new FileInfo(destPath);

			if (!dstInfo.Exists ||
				srcInfo.Length != dstInfo.Length ||
				srcInfo.LastWriteTimeUtc != dstInfo.LastWriteTimeUtc)
			{
				File.Copy(sourcePath, destPath, true);
				File.SetLastWriteTimeUtc(destPath, srcInfo.LastWriteTimeUtc);
			}
		}
		catch (Exception ex)
		{
			_log.Warn($"Could not update cached plugin file '{destPath}' from '{sourcePath}'.", ex);
		}
	}


	private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
	{
		if (assembly is null)
		{
			return [];
		}

		try
		{
			return assembly.GetTypes();
		}
		catch (ReflectionTypeLoadException ex)
		{
			return ex.Types.Where(t => t is not null);
		}
		catch
		{
			return [];
		}
	}


	private static Assembly? CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		if (string.IsNullOrWhiteSpace(args?.Name))
		{
			return null;
		}

		try
		{
			var requestedName = new AssemblyName(args!.Name).Name;

			if (!string.IsNullOrWhiteSpace(requestedName) &&
				_loadedAssembliesBySimpleName.TryGetValue(requestedName, out var asm))
			{
				return asm;
			}
		}
		catch { }

		return null;
	}
}
