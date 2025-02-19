using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Xml;
using mojoPortal.Web;

namespace mojoPortal.FileSystem;

public class FileSystemConfiguration
{
	public FileSystemConfiguration(XmlNode node)
	{
		LoadValuesFromConfigurationXml(node);
	}

	public ProviderSettingsCollection Providers { get; } = [];

	public string DefaultProvider { get; private set; } = "DiskFileSystemProvider";

	public void LoadValuesFromConfigurationXml(XmlNode node)
	{
		if (node.Attributes["defaultProvider"] is not null)
		{
			DefaultProvider = node.Attributes["defaultProvider"].Value;
		}

		foreach (XmlNode child in node.ChildNodes)
		{
			if (child.Name == "providers")
			{
				foreach (XmlNode providerNode in child.ChildNodes)
				{
					if (providerNode.NodeType == XmlNodeType.Element
						&& providerNode.Name == "add")
					{
						if (providerNode.Attributes["name"] is not null
							&& providerNode.Attributes["type"] is not null)
						{
							var providerSettings = new ProviderSettings(
								providerNode.Attributes["name"].Value,
								providerNode.Attributes["type"].Value);

							Providers.Add(providerSettings);
						}
					}
				}
			}
		}
	}

	public static FileSystemConfiguration GetConfig()
	{
		//FileSystemConfiguration editorConfig = null;

		if (HttpRuntime.Cache["mojoFileSystemConfig"] is not null
			&& HttpRuntime.Cache["mojoFileSystemConfig"] is FileSystemConfiguration configuration
		)
		{
			return configuration;
		}
		else
		{

			string configFileName = WebConfigSettings.mojoFileSystemConfigFileName;

			if (!configFileName.StartsWith("~/"))
			{
				configFileName = $"~/{configFileName}";
			}

			string pathToConfigFile = HostingEnvironment.MapPath(configFileName);

			var configXml = Core.Helpers.XmlHelper.GetXmlDocument(pathToConfigFile);
			//editorConfig = new FileSystemConfiguration(configXml.DocumentElement);

			var aggregateCacheDependency = new AggregateCacheDependency();
			aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));

			HttpRuntime.Cache.Insert(
				"mojoFileSystemConfig",
				//editorConfig,
				new FileSystemConfiguration(configXml.DocumentElement),
				aggregateCacheDependency,
				DateTime.Now.AddYears(1),
				TimeSpan.Zero,
				CacheItemPriority.Default,
				null);

			return (FileSystemConfiguration)HttpRuntime.Cache["mojoFileSystemConfig"];
		}
	}
}