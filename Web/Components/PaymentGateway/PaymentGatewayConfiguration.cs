using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace mojoPortal.Web.Commerce
{
	public class PaymentGatewayConfiguration
	{
		public ProviderSettingsCollection Providers { get; } = new ProviderSettingsCollection();
		public string DefaultProvider { get; private set; } = "NotImplementedPaymentGatewayProvider";


		#region Constructors

		public PaymentGatewayConfiguration(XmlNode node)
		{
			LoadValuesFromConfigurationXml(node);
		}

		#endregion



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
							providerNode.NodeType == XmlNodeType.Element &&
							providerNode.Name == "add"
						)
						{
							if (
								providerNode.Attributes["name"] != null &&
								providerNode.Attributes["type"] != null
							)
							{
								ProviderSettings providerSettings = new ProviderSettings(
									providerNode.Attributes["name"].Value,
									providerNode.Attributes["type"].Value
								);

								Providers.Add(providerSettings);
							}
						}
					}
				}
			}
		}


		public static PaymentGatewayConfiguration GetConfig()
		{
			PaymentGatewayConfiguration config = null;

			if (
				HttpRuntime.Cache["PaymentGatewayConfiguration"] != null &&
				HttpRuntime.Cache["PaymentGatewayConfiguration"] is PaymentGatewayConfiguration
			)
			{
				return (PaymentGatewayConfiguration)HttpRuntime.Cache["PaymentGatewayConfiguration"];
			}
			else
			{
				string configFileName = WebConfigSettings.PaymentGatewayConfigFileName;

				if (!configFileName.StartsWith("~/"))
				{
					configFileName = "~/" + configFileName;
				}

				string pathToConfigFile = HttpContext.Current.Server.MapPath(configFileName);

				var configXml = Core.Helpers.XmlHelper.GetXmlDocument(pathToConfigFile);
				config = new PaymentGatewayConfiguration(configXml.DocumentElement);

				var aggregateCacheDependency = new AggregateCacheDependency();

				aggregateCacheDependency.Add(new CacheDependency(pathToConfigFile));

				HttpRuntime.Cache.Insert(
					"PaymentGatewayConfiguration",
					config,
					aggregateCacheDependency,
					DateTime.Now.AddYears(1),
					TimeSpan.Zero,
					CacheItemPriority.Default,
					null
				);

				return (PaymentGatewayConfiguration)HttpRuntime.Cache["PaymentGatewayConfiguration"];
			}
		}
	}
}
