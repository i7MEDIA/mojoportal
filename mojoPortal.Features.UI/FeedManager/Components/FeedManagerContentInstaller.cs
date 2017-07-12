// Author:					
// Created:				    2011-03-23
// Last Modified:			2011-03-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI 
{
    public class FeedManagerContentInstaller : IContentInstaller
    {
        public void InstallContent(Module module, string configInfo)
        {
            if (string.IsNullOrEmpty(configInfo)) { return; }

            SiteSettings siteSettings = new SiteSettings(module.SiteId);
            SiteUser admin = SiteUser.GetNewestUser(siteSettings);

            XmlDocument xml = new XmlDocument();

            using (StreamReader stream = File.OpenText(HostingEnvironment.MapPath(configInfo)))
            {
                xml.LoadXml(stream.ReadToEnd());
            }

            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
                if (node.Name == "feed")
                {
                    XmlAttributeCollection feedAttributes = node.Attributes;

                    RssFeed feed = new RssFeed(module.ModuleId);

                    feed.ModuleId = module.ModuleId;
                    feed.ModuleGuid = module.ModuleGuid;

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
                        if (int.TryParse(feedAttributes["sortRank"].Value,
                            out sort))
                        {
                            feed.SortRank = sort;
                        }
                    }
                   
                    feed.Save();
                }

                if (node.Name == "moduleSetting")
                {
                    XmlAttributeCollection settingAttributes = node.Attributes;

                    if ((settingAttributes["settingKey"] != null)&&(settingAttributes["settingKey"].Value.Length > 0))
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