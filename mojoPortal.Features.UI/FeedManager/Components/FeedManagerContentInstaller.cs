using mojoPortal.Business;
using mojoPortal.Web;
using System.IO;
using System.Web.Hosting;
using System.Xml;

namespace mojoPortal.Features.UI
{
	public class FeedManagerContentInstaller : IContentInstaller
	{
		public void InstallContent(Module module, string configInfo)
		{
			if (string.IsNullOrEmpty(configInfo))
			{
				return;
			}

			SiteSettings siteSettings = new SiteSettings(module.SiteId);
			SiteUser admin = SiteUser.GetNewestUser(siteSettings);

			FileStream stream = File.OpenRead(HostingEnvironment.MapPath(configInfo));
			var xml = Core.Helpers.XmlHelper.GetXmlDocument(stream);

			foreach (XmlNode node in xml.DocumentElement.ChildNodes)
			{
				if (node.Name == "feed")
				{
					XmlAttributeCollection feedAttributes = node.Attributes;

					RssFeed feed = new RssFeed(module.ModuleId)
					{
						ModuleId = module.ModuleId,
						ModuleGuid = module.ModuleGuid
					};

					if (admin != null)
					{
						feed.UserId = admin.UserId;
						feed.UserGuid = admin.UserGuid;
						feed.LastModUserGuid = admin.UserGuid;
					}

					if (feedAttributes["feedName"] != null)
					{
						feed.Author = feedAttributes["feedName"].Value;
					}

					if (feedAttributes["webUrl"] != null)
					{
						feed.Url = feedAttributes["webUrl"].Value;
					}


					if (feedAttributes["feedUrl"] != null)
					{
						feed.RssUrl = feedAttributes["feedUrl"].Value;
					}

					if (feedAttributes["sortRank"] != null)
					{
						int sort = 500;
						if (int.TryParse(feedAttributes["sortRank"].Value, out sort))
						{
							feed.SortRank = sort;
						}
					}

					feed.Save();
				}

				if (node.Name == "moduleSetting")
				{
					XmlAttributeCollection settingAttributes = node.Attributes;

					if ((settingAttributes["settingKey"] != null) && (settingAttributes["settingKey"].Value.Length > 0))
					{
						string key = settingAttributes["settingKey"].Value;
						string val = string.Empty;
						if (settingAttributes["settingValue"] != null)
						{
							val = settingAttributes["settingValue"].Value;
						}

						ModuleSettings.UpdateModuleSetting(module.ModuleGuid, module.ModuleId, key, val);
					}
				}
			}
		}
	}
}
