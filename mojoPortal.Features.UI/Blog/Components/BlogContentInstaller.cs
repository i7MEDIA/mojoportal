// Author:					
// Created:				    2011-03-23
// Last Modified:			2011-04-03
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
using System.Xml;
using System.Text;
using System.Web.Hosting;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI
{
    public class BlogContentInstaller : IContentInstaller
    {
        public void InstallContent(Module module, string configInfo)
        {
            if (string.IsNullOrEmpty(configInfo)) { return; }

            SiteSettings siteSettings = new SiteSettings(module.SiteId);
            SiteUser admin = SiteUser.GetNewestUser(siteSettings);

            FileStream stream = File.OpenRead(HostingEnvironment.MapPath(configInfo));
			var xml = Core.Helpers.XmlHelper.GetXmlDocument(stream);

			XmlNode postsNode = null;
            foreach (XmlNode n in xml.DocumentElement.ChildNodes)
            {
                if (n.Name == "posts")
                {
                    postsNode = n;
                    break;
                }
            }

            if (postsNode != null)
            {
                foreach (XmlNode node in postsNode.ChildNodes)
                {
                    if (node.Name == "post")
                    {
                        XmlAttributeCollection postAttrributes = node.Attributes;
                        

                        Blog b = new Blog();
                        b.ModuleGuid = module.ModuleGuid;
                        b.ModuleId = module.ModuleId;

                        if ((postAttrributes["title"] != null) && (postAttrributes["title"].Value.Length > 0))
                        {
                            b.Title = postAttrributes["title"].Value;
                        }

                        b.ItemUrl = "~/" + SiteUtils.SuggestFriendlyUrl(b.Title, siteSettings);

                        foreach (XmlNode descriptionNode in node.ChildNodes)
                        {
                            if (descriptionNode.Name == "excerpt")
                            {
                                b.Excerpt = descriptionNode.InnerText;
                                break;
                            }
                        }

                        foreach (XmlNode descriptionNode in node.ChildNodes)
                        {
                            if (descriptionNode.Name == "description")
                            {
                                b.Description = descriptionNode.InnerText;
                                break;
                            }
                        }

                        b.UserGuid = admin.UserGuid;

                        if (b.Save())
                        {
                            FriendlyUrl newFriendlyUrl = new FriendlyUrl();
                            newFriendlyUrl.SiteId = siteSettings.SiteId;
                            newFriendlyUrl.SiteGuid = siteSettings.SiteGuid;
                            newFriendlyUrl.PageGuid = b.BlogGuid;
                            newFriendlyUrl.Url = b.ItemUrl.Replace("~/", string.Empty); ;
                            newFriendlyUrl.RealUrl = "~/Blog/ViewPost.aspx?pageid="
                                + module.PageId.ToInvariantString()
                                + "&mid=" + b.ModuleId.ToInvariantString()
                                + "&ItemID=" + b.ItemId.ToInvariantString();

                            newFriendlyUrl.Save();

                            
                        }

                    }
                }
            }

           

            

            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
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