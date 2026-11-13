#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;

namespace mojoPortal.Web.Plugins;

public class PluginDescriptor
{
	public IPlugin? Instance { get; set; }
	public Guid Id => Instance?.Id ?? Guid.Empty;
	public string Name => Instance?.Name ?? FolderName;
	public string LegacyApplicationName => Instance?.LegacyApplicationName ?? Name;
	public Version Version => Instance?.Version ?? new Version(0, 0, 0, 0);
	public string FolderName { get; set; } = string.Empty;
	public string PhysicalPath { get; set; } = string.Empty;
	public string VirtualRootPath => $"~/Plugins/{FolderName}";
	public int EffectiveOrder { get; set; } = 100;
	public bool Enabled { get; set; } = true;
	public List<Assembly> Assemblies { get; } = [];
	public Assembly? Assembly => Instance?.GetType().Assembly;
}
