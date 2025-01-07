using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace mojoPortal.Web.Controls.Captcha;

public class CaptchaConfiguration
{
	private readonly ProviderSettingsCollection providerSettingsCollection = [];

	public string DefaultProvider { get; private set; } = "SimpleMathCaptchaProvider";


	public CaptchaConfiguration(XmlNode node) => LoadValuesFromConfigurationXml(node);
	public ProviderSettingsCollection Providers => providerSettingsCollection;


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
						providerNode.Name == "add"
					)
					{
						if (
							providerNode.Attributes["name"] is not null &&
							providerNode.Attributes["type"] is not null
						)
						{
							var providerSettings = new ProviderSettings(
								providerNode.Attributes["name"].Value,
								providerNode.Attributes["type"].Value
							);

							providerSettingsCollection.Add(providerSettings);
						}
					}
				}
			}
		}
	}


	public static CaptchaConfiguration GetConfig()
	{
		if (HttpRuntime.Cache["mojoCaptchaConfig"] is CaptchaConfiguration configuration)
		{
			return configuration;
		}
		else
		{
			var configFileName = "mojoCaptcha.config";

			if (ConfigurationManager.AppSettings["mojoCaptchaConfigFileName"] is not null)
			{
				configFileName = ConfigurationManager.AppSettings["mojoCaptchaConfigFileName"];
			}

			if (!configFileName.StartsWith("~/"))
			{
				configFileName = "~/" + configFileName;
			}

			var pathToConfigFile = HttpContext.Current.Server.MapPath(configFileName);
			var configXml = Core.Helpers.XmlHelper.GetXmlDocument(pathToConfigFile);
			var captchaConfig = new CaptchaConfiguration(configXml.DocumentElement);
			var aggregateCacheDependency = new AggregateCacheDependency();

			aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));

			HttpRuntime.Cache.Insert(
				"mojoCaptchaConfig",
				captchaConfig,
				aggregateCacheDependency,
				DateTime.Now.AddYears(1),
				TimeSpan.Zero,
				CacheItemPriority.Default,
				null
			);

			return (CaptchaConfiguration)HttpRuntime.Cache["mojoCaptchaConfig"];
		}
	}
}
