/// Original Author:	Joseph Hill
/// Created:			2006-01-09
/// Last Modified:		2013-02-26
/// 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Argotic.Syndication;

namespace mojoPortal.Web.ForumUI
{
	public partial class RssForumFeed : Page
	{
		private int maxDaysOld = 90;
        private int moduleId = -1;
        private int pageId = -1;
        private int itemId = -1;
        private int threadId = -1;
        private string navigationSiteRoot = string.Empty;
        private string imageSiteRoot = string.Empty;
        private string forumUrl = string.Empty;
        private string cssBaseUrl = string.Empty;
        private int entriesLimit = 90;
        private int entryCount = 0;
        private string baseUrl = string.Empty;
        private SiteSettings siteSettings = null;
        private PageSettings pageSettings = null;
        //private Module module = null;
        private Hashtable moduleSettings = null;
        private Guid securityBypassGuid = Guid.Empty;
        //private bool canView = false;
        private bool bypassPageSecurity = false;
        private bool isGoogleFeedReader = false;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            LoadSettings();


            //if (canView)
            //{

               RenderRss();

            //}
            //else
            //{
            //    RenderError("Invalid Request");
            //}
        }

        private void RenderRss()
        {
            if (pageSettings == null) { return; }

            RssForum rssForum = new RssForum();
            rssForum.SiteId = siteSettings.SiteId;
            rssForum.ModuleId = moduleId;
            rssForum.PageId = pageId;
            rssForum.ItemId = itemId;
            rssForum.ThreadId = threadId;
            rssForum.MaximumDays = maxDaysOld;
          
            Argotic.Syndication.RssFeed feed = new Argotic.Syndication.RssFeed();
            RssChannel channel = new RssChannel();
            channel.Generator = "mojoPortal Forum module";

            string channelLink = WebUtils.ResolveServerUrl(SiteUtils.GetPageUrl(pageSettings));
            channel.Link = new System.Uri(channelLink);

            // if (config.ChannelDescription.Length > 0) { channel.Description = config.ChannelDescription; }

            feed.Channel = channel;

            string target = navigationSiteRoot + "/Forums/Thread.aspx?pageid=";
            
            using (IDataReader posts = rssForum.GetPostsForRss())
            {
                while ((posts.Read()) && (entryCount <= entriesLimit))
                {

                    string pageViewRoles = posts["AuthorizedRoles"].ToString();
                    string moduleViewRoles = posts["ViewRoles"].ToString();
                    bool include = (bypassPageSecurity ||
                        (pageViewRoles.Contains("All Users"))
                        && ((moduleViewRoles.Length == 0) || (moduleViewRoles.Contains("All Users")))
                        )
                        ;

                    if (!include) { continue; }

                    RssItem item = new RssItem();

                    item.Title = posts["Subject"].ToString();
                    item.Description = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(navigationSiteRoot, imageSiteRoot, posts["Post"].ToString());
                    item.PublicationDate = Convert.ToDateTime(posts["PostDate"]);
                    
                    int p = Convert.ToInt32(posts["PageID"]);
                    int t = Convert.ToInt32(posts["ThreadID"]);

                    //if (target.IndexOf("&thread=") < 0 && target.IndexOf("?thread=") < 0)
                    //{
                    //    if (target.IndexOf("?") < 0)
                    //    {
                    //        target += "?thread=" + posts["ThreadID"].ToString() + "#post" + posts["PostID"].ToString();
                    //    }
                    //    else
                    //    {
                    //        target += "&thread=" + posts["ThreadID"].ToString() + "#post" + posts["PostID"].ToString();
                    //    }
                    //}

                    //https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11417~1#post47516
                    if ((isGoogleFeedReader) && (ForumConfiguration.UseOldParamsForGoogleReader))
                    {

                        //item.Link = new System.Uri(target + p.ToInvariantString() + "&thread=" + t.ToInvariantString() + "#post" + posts["PostID"].ToString());
                        item.Link = new System.Uri(target + p.ToInvariantString() + "&thread=" + t.ToInvariantString());
                    }
                    else
                    {
                        item.Link = new System.Uri(target + p.ToInvariantString() + "&t=" + t.ToInvariantString() + "~-1#post" + posts["PostID"].ToString());
                    }

                    RssGuid g = new RssGuid(target, true);
                    item.Guid = g;

                    item.Author = posts["StartedBy"].ToString();

                    channel.AddItem(item);
                    entryCount += 1;

                }
            }

            Response.Cache.SetExpires(DateTime.Now.AddMinutes(5));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.ContentType = "application/xml";

            Encoding encoding = new UTF8Encoding();
            Response.ContentEncoding = encoding;

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(Response.OutputStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.Indented;

                //////////////////
                // style for RSS Feed viewed in browsers
                if (ConfigurationManager.AppSettings["RSSCSS"] != null)
                {
                    string rssCss = ConfigurationManager.AppSettings["RSSCSS"].ToString();
                    xmlTextWriter.WriteWhitespace(" ");
                    xmlTextWriter.WriteRaw("<?xml-stylesheet type=\"text/css\" href=\"" + cssBaseUrl + rssCss + "\" ?>");

                }

                if (ConfigurationManager.AppSettings["RSSXsl"] != null)
                {
                    string rssXsl = ConfigurationManager.AppSettings["RSSXsl"].ToString();
                    xmlTextWriter.WriteWhitespace(" ");
                    xmlTextWriter.WriteRaw("<?xml-stylesheet type=\"text/xsl\" href=\"" + cssBaseUrl + rssXsl + "\" ?>");

                }
                ///////////////////////////

               
                feed.Save(xmlTextWriter);

            }

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            pageSettings = CacheHelper.GetCurrentPage();
            //pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            //moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            //itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            //threadId = WebUtils.ParseInt32FromQueryString("thread", -1);

            FeedParameterParser feedParams = new FeedParameterParser();
            feedParams.Parse();

            pageId = feedParams.PageId;
            moduleId = feedParams.ModuleId;
            itemId = feedParams.ItemId;
            threadId = feedParams.ThreadId;
            
            // old format still supported for backward compat
            //  /Forums/RSS.aspx?mid=34&pageid=5
            //  /Forums/RSS.aspx?ItemID=2&mid=34&pageid=5
            //  /Forums/RSS.aspx?ItemID=2&mid=34&pageid=5&thread=10373

            // new format reduces to 2 params with the 2nd one a combination of ~ delimited: moduelid~itemId~threadid
            //  /Forums/RSS.aspx?pageid=5&m=34~-1~-1
            
            
            securityBypassGuid = WebUtils.ParseGuidFromQueryString("g", securityBypassGuid);
            //module = GetModule();

            //if ((moduleId == -1) || (module == null) || (siteSettings == null)) { return; }

            bypassPageSecurity = false;

            if ((securityBypassGuid != Guid.Empty) && (securityBypassGuid == WebConfigSettings.InternalFeedSecurityBypassKey))
            {
                bypassPageSecurity = true;
            }
           
            // this old security logic is not needed since we are filtering the items by page and module roles

            //if(
            //    (bypassPageSecurity)
            //    ||(WebUser.IsInRoles(pageSettings.AuthorizedRoles))
            //    || (WebUser.IsInRoles(module.ViewRoles))
            //    )
            //{
            //    canView = true;
            //}

            //if (!canView) { return; }

            //baseUrl = Request.Url.ToString().Replace("RSS.aspx", "Thread.aspx");

            if (WebConfigSettings.UseFolderBasedMultiTenants)
            {
                navigationSiteRoot = SiteUtils.GetNavigationSiteRoot();
                imageSiteRoot = WebUtils.GetSiteRoot();
                cssBaseUrl = imageSiteRoot;

            }
            else
            {
                navigationSiteRoot = SiteUtils.GetNavigationSiteRoot();
                imageSiteRoot = navigationSiteRoot;
                cssBaseUrl = WebUtils.GetSiteRoot();

            }

           
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            maxDaysOld = WebUtils.ParseInt32FromHashtable(moduleSettings, "RSSFeedMaxDaysOldSetting", 90);

            entriesLimit = WebUtils.ParseInt32FromHashtable( moduleSettings, "RSSFeedMaxPostsSetting", 90);

            //Feedfetcher-Google
            isGoogleFeedReader = ((Request.UserAgent != null) && (Request.UserAgent.Contains("Feedfetcher-Google")));
                

           
        }

       


		private void RenderError(string message)
		{
			Response.Write(message);
		}

		private Module GetModule() 
		{
			
            if (pageSettings != null)
            {
                foreach (Module module in pageSettings.Modules)
                {
                    if (module.ModuleId == moduleId)
                        return module;
                }
            }
			return null;
		}

		
	}
}
