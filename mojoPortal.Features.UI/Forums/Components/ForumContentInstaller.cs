// Author:					
// Created:				    2011-03-23
// Last Modified:			2011-03-24
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
    public class ForumContentInstaller : IContentInstaller
    {
        public void InstallContent(Module module, string configInfo)
        {
            if (string.IsNullOrEmpty(configInfo)) { return; }

            int userId = SiteUser.GetNewestUserId(module.SiteId);

			FileStream stream = File.OpenRead(HostingEnvironment.MapPath(configInfo));
			var xml = Core.Helpers.XmlHelper.GetXmlDocument(stream);

			foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
                if (node.Name == "forum")
                {
                    Forum forum = new Forum();
                    forum.ModuleId = module.ModuleId;
                    

                    XmlAttributeCollection attributeCollection = node.Attributes;

                    if (attributeCollection["title"] != null)
                    {
                        forum.Title = attributeCollection["title"].Value;
                    }

                    if (attributeCollection["sortOrder"] != null)
                    {
                        int sort = 1;
                        if (int.TryParse(attributeCollection["sortOrder"].Value,
                            out sort))
                        {
                            forum.SortOrder = sort;
                        }
                    }

                    foreach (XmlNode descriptionNode in node.ChildNodes)
                    {
                        if (descriptionNode.Name == "description")
                        {
                            forum.Description = descriptionNode.InnerText;
                            break;
                        }
                    }

                    forum.CreatedByUserId = userId;
                    
                    forum.Save();

                    foreach (XmlNode threadsNode in node.ChildNodes)
                    {
                        if (threadsNode.Name == "threads")
                        {

                            foreach (XmlNode threadNode in threadsNode.ChildNodes)
                            {
                                if (threadNode.Name == "thread")
                                {
                                    XmlAttributeCollection threadAttributes = threadNode.Attributes;

                                    ForumThread thread = new ForumThread();
                                    thread.ForumId = forum.ItemId;
                                    thread.PostUserId = userId;

                                    if (threadAttributes["subject"] != null)
                                    {
                                        thread.PostSubject = threadAttributes["subject"].Value;
                                    }

                                    foreach (XmlNode postNode in threadNode.ChildNodes)
                                    {
                                        if (postNode.Name == "post")
                                        {
                                            thread.PostMessage = postNode.InnerText;
                                            break; //TODO: this is limited to one post when creating a thread. could support more but just making it for the demo site
                                        }
                                    }

                                    thread.Post();

                                }
                            }

                            break; //there should only be one threads node
                        }
                    }

                }
            }

           

        }
    }
}