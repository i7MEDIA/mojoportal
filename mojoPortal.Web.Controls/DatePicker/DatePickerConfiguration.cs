using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace mojoPortal.Web.Controls.DatePicker
{
	public class DatePickerConfiguration
	{
		public DatePickerConfiguration(XmlNode node)
		{
			LoadValuesFromConfigurationXml(node);
		}

		public ProviderSettingsCollection Providers { get; } = new ProviderSettingsCollection();

		public string DefaultProvider { get; private set; } = "jsCalendarDatePickerProvider";

		public void LoadValuesFromConfigurationXml(XmlNode node)
		{
			if (node.Attributes["defaultProvider"] != null)
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
							providerNode.NodeType == XmlNodeType.Element
							&& providerNode.Name == "add"
						)
						{
							if (
								providerNode.Attributes["name"] != null
								&& providerNode.Attributes["type"] != null
							)
							{
								ProviderSettings providerSettings
									= new ProviderSettings(
									providerNode.Attributes["name"].Value,
									providerNode.Attributes["type"].Value);

								Providers.Add(providerSettings);
							}
						}
					}
				}
			}
		}

		public static DatePickerConfiguration GetConfig()
		{
			DatePickerConfiguration datePickerConfig;

			if (
				HttpRuntime.Cache["mojoDatePickerConfig"] != null
				&& HttpRuntime.Cache["mojoDatePickerConfig"] is DatePickerConfiguration configuration
			)
			{
				return configuration;
			}
			else
			{
				string configFileName = "mojoDatePicker.config";
				if (ConfigurationManager.AppSettings["mojoDatePickerConfigFileName"] != null)
				{
					configFileName = ConfigurationManager.AppSettings["mojoDatePickerConfigFileName"];
				}

				if (!configFileName.StartsWith("~/"))
				{
					configFileName = "~/" + configFileName;
				}

				string pathToConfigFile = HttpContext.Current.Server.MapPath(configFileName);

				var configXml = Core.Helpers.XmlHelper.GetXmlDocument(pathToConfigFile);

				datePickerConfig = new DatePickerConfiguration(configXml.DocumentElement);

				var aggregateCacheDependency = new AggregateCacheDependency();
				aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));

				HttpRuntime.Cache.Insert(
					"mojoDatePickerConfig",
					datePickerConfig,
					aggregateCacheDependency,
					DateTime.Now.AddYears(1),
					TimeSpan.Zero,
					CacheItemPriority.Default,
					null);

				return (DatePickerConfiguration)HttpRuntime.Cache["mojoDatePickerConfig"];
			}
		}
	}
}
