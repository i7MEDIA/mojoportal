using mojoPortal.Core.Helpers;
using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace mojoPortal.Web.Editor;

public class EditorConfiguration
{
	public string DefaultProvider { get; private set; } = "CKeditorProvider";
	public ProviderSettingsCollection Providers { get; } = [];


	public EditorConfiguration(XmlNode node)
	{
		LoadValuesFromConfigurationXml(node);
	}


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
					if (
						providerNode.NodeType == XmlNodeType.Element &&
						providerNode.Name == "add")
					{
						if (
							providerNode.Attributes["name"] is not null &&
							providerNode.Attributes["type"] != null
						)
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


	public static EditorConfiguration GetConfig()
	{
		if (HttpRuntime.Cache["mojoEditorConfig"] is not null and EditorConfiguration configuration)
		{
			return configuration;
		}
		else
		{
			var configFileName = "mojoEditor.config";

			if (ConfigurationManager.AppSettings["mojoEditorConfigFileName"] is not null)
			{
				configFileName = ConfigurationManager.AppSettings["mojoEditorConfigFileName"];
			}

			if (!configFileName.StartsWith("~/"))
			{
				configFileName = $"~/{configFileName}";
			}

			var pathToConfigFile = HttpContext.Current.Server.MapPath(configFileName);
			var configXml = XmlHelper.GetXmlDocument(pathToConfigFile);
			var editorConfig = new EditorConfiguration(configXml.DocumentElement);
			var aggregateCacheDependency = new AggregateCacheDependency();

			aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));

			HttpRuntime.Cache.Insert(
				"mojoEditorConfig",
				editorConfig,
				aggregateCacheDependency,
				DateTime.Now.AddYears(1),
				TimeSpan.Zero,
				CacheItemPriority.Default,
				null);

			return HttpRuntime.Cache["mojoEditorConfig"] as EditorConfiguration;
		}
	}
}
