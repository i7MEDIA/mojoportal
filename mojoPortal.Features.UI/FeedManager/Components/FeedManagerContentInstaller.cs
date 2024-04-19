using System.IO;
using System.Web.Hosting;
using System.Xml;
using mojoPortal.Business;

namespace mojoPortal.Features.UI;

public class FeedManagerContentInstaller : IContentInstaller
{
	public void InstallContent(Module module, string configInfo)
	{
		if (string.IsNullOrEmpty(configInfo))
		{
			return;
		}

		var siteSettings = new SiteSettings(module.SiteId);
		var admin = SiteUser.GetNewestUser(siteSettings);

		var stream = File.OpenRead(HostingEnvironment.MapPath(configInfo));
		var xml = XmlHelper.GetXmlDocument(stream);

		foreach (XmlNode node in xml.DocumentElement.ChildNodes)
		{
			if (node.Name == "feed")
			{
				var feedAttributes = node.Attributes;

				var feed = new RssFeed(module.ModuleId)
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

				feed.Author = feedAttributes.ParseStringFromAttribute("feedName", feed.Author);
				feed.Url = feedAttributes.ParseStringFromAttribute("webUrl", feed.Url);
				feed.RssUrl = feedAttributes.ParseStringFromAttribute("feedUrl", feed.RssUrl);

				if (feedAttributes["sortRank"] != null)
				{
					if (int.TryParse(feedAttributes["sortRank"].Value, out int sort))
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
