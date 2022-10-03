///		Author:					
///		Created:				2005-03-15
///		Last Modified:			2017-03-15

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Argotic.Syndication;
using Argotic.Extensions.Core;
using mojoPortal.Web.BlogUI;


namespace mojoPortal.Web.BlogUI
{

    public partial class RssPage : Page
    {
        private int moduleId = -1;
        private int pageID = -1;
        private int categoryId = -1;
        private int totalPages = 0;
        private SiteSettings siteSettings = null;
        private PageSettings pageSettings = null;
        private Module module = null;
        Hashtable moduleSettings = null;
        private BlogConfiguration config = new BlogConfiguration();

        private string navigationSiteRoot = string.Empty;
        private string blogBaseUrl = string.Empty;
        private string imageSiteRoot = string.Empty;
        private string cssBaseUrl = string.Empty;
        private Guid securityBypassGuid = Guid.Empty;
        private bool canView = false;
        private bool shouldRedirectToFeedburner = false;
        private string attachmentBaseUrl = string.Empty;
        //private int timeToLive = -1;


        protected void Page_Load(object sender, System.EventArgs e)
        {
            // nothing should post here
            if (Page.IsPostBack) return;

			LoadSettings();

			if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || pageSettings.RequireSsl))
            {
                SiteUtils.ForceSsl();
            }
            else
            {
                SiteUtils.ClearSsl();
            }


            if (canView)
            {
                if (shouldRedirectToFeedburner)
                {
                    Response.Redirect(config.FeedburnerFeedUrl, true);
                }
                else
                {
                    RenderRss();
                }

            }
            else
            {
                RenderError("Invalid Request");
            }

        }

        private DataSet GetData()
        {
            if (categoryId == -1)
            {
                return Blog.GetPageDataSet(
                    moduleId, 
                    DateTime.UtcNow, 
                    1, 
                    config.MaxFeedItems, 
                    out totalPages);
            }

            return Blog.GetBlogEntriesByCategory(
                        moduleId,
                        categoryId,
                        DateTime.UtcNow,
                        1,
                        config.MaxFeedItems,
                        out totalPages);

        }


        private void RenderRss()
        {
            Argotic.Syndication.RssFeed feed = new Argotic.Syndication.RssFeed();
            RssChannel channel = new RssChannel();
            channel.Generator = "mojoPortal Blog Module";

            channel.Language = SiteUtils.GetDefaultCulture();
            feed.Channel = channel;
            if (module.ModuleTitle.Length > 0)
            {
                channel.Title = module.ModuleTitle;
            }
            else
            {
                channel.Title = "Blog"; // it will cause an error if this is left blank so we must populate it if the module title is an emty string.
            }

            // this became broken when we combined query string params, since pageid is not one of the params this always returns the home page url
            // instead of the blog page url
            //string pu = WebUtils.ResolveServerUrl(SiteUtils.GetCurrentPageUrl());

            string pu = WebUtils.ResolveServerUrl(SiteUtils.GetPageUrl(pageSettings));


            channel.Link = new System.Uri(pu);
            channel.SelfLink = Request.Url;


            if (config.ChannelDescription.Length > 0) { channel.Description = config.ChannelDescription; }
            if (config.Copyright.Length > 0) { channel.Copyright = config.Copyright; }

            channel.ManagingEditor = config.ManagingEditorEmail;
            if (config.ManagingEditorName.Length > 0)
            {
                channel.ManagingEditor += " (" + config.ManagingEditorName + ")";
            }
            
            
            if (config.FeedTimeToLive > -1) { channel.TimeToLive = config.FeedTimeToLive; }


            //  Create and add iTunes information to feed channel
            ITunesSyndicationExtension channelExtension = new ITunesSyndicationExtension();
            channelExtension.Context.Subtitle = config.ChannelDescription;

            
            if (config.HasExplicitContent)
            {
                channelExtension.Context.ExplicitMaterial = ITunesExplicitMaterial.Yes;
            }
            else
            {
                channelExtension.Context.ExplicitMaterial = ITunesExplicitMaterial.No;
            }

            channelExtension.Context.Author = config.ManagingEditorEmail;
            if (config.ManagingEditorName.Length > 0)
            {
                channelExtension.Context.Author += " (" + config.ManagingEditorName + ")";
            }
            channelExtension.Context.Summary = config.ChannelDescription;
            channelExtension.Context.Owner = new ITunesOwner(config.ManagingEditorEmail, config.ManagingEditorName);
            

  
            if (config.FeedLogoUrl.Length > 0)
            {
                try
                {
                    channelExtension.Context.Image = new Uri(config.FeedLogoUrl);
                }
                catch (ArgumentNullException) { }
                catch (UriFormatException) { }
            }


            if (config.FeedMainCategory.Length > 0)
            {
                ITunesCategory mainCat = new ITunesCategory(config.FeedMainCategory);

                if (config.FeedSubCategory.Length > 0)
                {
                    mainCat.Categories.Add(new ITunesCategory(config.FeedSubCategory));
                }

                
                channelExtension.Context.Categories.Add(mainCat);
            }
            

            feed.Channel.AddExtension(channelExtension);


            DataSet dsBlogPosts = GetData();

            DataTable posts = dsBlogPosts.Tables["Posts"];

           
            foreach (DataRow dr in posts.Rows)
            {
                bool inFeed = Convert.ToBoolean(dr["IncludeInFeed"]);
                if (!inFeed) { continue; }

                RssItem item = new RssItem();

                int itemId = Convert.ToInt32(dr["ItemID"]);
                string blogItemUrl = FormatBlogUrl(dr["ItemUrl"].ToString(), itemId);
                item.Link = new Uri(Request.Url, blogItemUrl);
                item.Guid = new RssGuid(blogItemUrl);
                item.Title = dr["Heading"].ToString();
                item.PublicationDate = Convert.ToDateTime(dr["StartDate"]);

                bool showAuthor = Convert.ToBoolean(dr["ShowAuthorName"]);
                if (showAuthor)
                {
                   
                    // techically this is supposed to be an email address
                    // but wouldn't that lead to a lot of spam?

                    string authorEmail = dr["Email"].ToString();
                    string authorName = dr["Name"].ToString();

                    if (BlogConfiguration.IncludeAuthorEmailInFeed)
                    {
                        item.Author = authorEmail + " (" + authorName + ")";
                    }
                    else
                    {

                        item.Author = authorName;
                    }
                }
                else if (config.ManagingEditorEmail.Length > 0)
                {
                    item.Author = config.ManagingEditorEmail;
                }

                item.Comments = new Uri(blogItemUrl);

                string signature = string.Empty;

                if (config.AddSignature)
                {
                    signature = "<br /><a href='" + blogItemUrl + "'>" + dr["Name"].ToString() + "</a>";
                }

                if ((config.AddCommentsLinkToFeed) && (config.AllowComments))
                {
                    signature += "&nbsp;&nbsp;" + "<a href='" + blogItemUrl + "'>...</a>";
                }

                if (config.AddTweetThisToFeed)
                {
                    signature += GenerateTweetThisLink(item.Title, blogItemUrl);

                }

                if (config.AddFacebookLikeToFeed)
                {
                    signature += GenerateFacebookLikeButton(blogItemUrl);

                }


                string blogPost = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(navigationSiteRoot, imageSiteRoot, dr["Description"].ToString().RemoveCDataTags());

                string staticMapLink = BuildStaticMapMarkup(dr);

                if (staticMapLink.Length > 0)
                {
                    // add a google static map
                    blogPost += staticMapLink;

                }


                if ((!config.UseExcerptInFeed) || (blogPost.Length <= config.ExcerptLength))
                {
                    item.Description = blogPost + signature;
                }
                else
                {
                    string excerpt = SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(navigationSiteRoot, imageSiteRoot, dr["Abstract"].ToString().RemoveCDataTags());

                    if ((excerpt.Length > 0) && (excerpt != "<p>&#160;</p>"))
                    {
                        excerpt = excerpt
                            + config.ExcerptSuffix
                            + " <a href='"
                            + blogItemUrl + "'>" + config.MoreLinkText + "</a><div class='excerptspacer'>&nbsp;</div>";
                    }
                    else
                    {
                        excerpt = UIHelper.CreateExcerpt(dr["Description"].ToString(), config.ExcerptLength, config.ExcerptSuffix)
                            + " <a href='"
                            + blogItemUrl + "'>" + config.MoreLinkText + "</a><div class='excerptspacer'>&nbsp;</div>"; ;
                    }

                    item.Description = excerpt;


                }

               

                // how to add media enclosures for podcasting
                //http://www.podcast411.com/howto_1.html

                //http://argotic.codeplex.com/wikipage?title=Generating%20an%20extended%20RSS%20feed

                //http://techwhimsy.com/stream-mp3s-with-google-mp3-player


                //Uri url = new Uri("http://media.libsyn.com/media/podcast411/411_060325.mp3");
                //string type = "audio/mpeg";
                //long length = 11779397;

                string blogGuid = dr["BlogGuid"].ToString();
                string whereClause = string.Format("ItemGuid = '{0}'", blogGuid);


                if (!config.UseExcerptInFeed)
                {
                    
                    DataView dv = new DataView(dsBlogPosts.Tables["Attachments"], whereClause, "", DataViewRowState.CurrentRows);

                    foreach (DataRowView rowView in dv)
                    {
                        DataRow row = rowView.Row;

                        Uri mediaUrl = new Uri(WebUtils.ResolveServerUrl(attachmentBaseUrl + row["ServerFileName"].ToString()));
                        long contentLength = Convert.ToInt64(row["ContentLength"]);
                        string contentType = row["ContentType"].ToString();

                        RssEnclosure enclosure = new RssEnclosure(contentLength, contentType, mediaUrl);
                        item.Enclosures.Add(enclosure);

                        //http://argotic.codeplex.com/wikipage?title=Generating%20an%20extended%20RSS%20feed

                        ITunesSyndicationExtension itemExtension = new ITunesSyndicationExtension();
                        itemExtension.Context.Author = dr["Name"].ToString();
                        itemExtension.Context.Subtitle = dr["SubTitle"].ToString();
                        //itemExtension.Context.Summary = "The iTunes syndication extension properties that are used vary based on whether extending the channel or an item";
                        //itemExtension.Context.Duration = new TimeSpan(1, 2, 13);
                        //itemExtension.Context.Keywords.Add("Podcast");
                        //itemExtension.Context.Keywords.Add("iTunes");


                        whereClause = string.Format("ItemID = '{0}'", itemId);
                        DataView dvCat = new DataView(dsBlogPosts.Tables["Categories"], whereClause, "", DataViewRowState.CurrentRows);

                        foreach (DataRowView rView in dvCat)
                        {
                            DataRow r = rView.Row;

                            item.Categories.Add(new RssCategory(r["Category"].ToString()));

                            itemExtension.Context.Keywords.Add(r["Category"].ToString());

                        }

                        item.AddExtension(itemExtension);
                    }


                }

                
                

                channel.AddItem(item);

            }
            

            if ((config.FeedburnerFeedUrl.Length > 0) || (Request.Url.AbsolutePath.Contains("localhost")))
            {
                Response.Cache.SetExpires(DateTime.Now.AddMinutes(-30));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.VaryByParams["r"] = true;
            }
            else
            {
                Response.Cache.SetExpires(DateTime.Now.AddMinutes(30));
                Response.Cache.SetCacheability(HttpCacheability.Public);
            }


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

        //https://developers.google.com/maps/documentation/staticmaps/

        //http://msdn.microsoft.com/en-us/library/ff701724.aspx //bing

        private string BuildStaticMapMarkup(DataRow dr)
        {
            if (siteSettings == null) { return string.Empty; }
            if (siteSettings.GmapApiKey.Length == 0) { return string.Empty; }

            string location = dr["Location"].ToString();

            if (location.Length == 0) { return location; }

            string mapHeight = dr["MapHeight"].ToString().Replace("px", string.Empty);
            string mapWidth = dr["MapWidth"].ToString().Replace("px", string.Empty).Replace("%",string.Empty);
            string mapType;
            string mapZoom = Convert.ToInt32(dr["MapZoom"]).ToInvariantString();


            // this experiment did not work it returns a json result not a static image
            //bool useBing = Convert.ToBoolean(dr["UseBingMap"]);

            //if (useBing && !WebConfigSettings.DisableBingStaticMaps && (WebConfigSettings.BingMapsApiKey.Length > 0))
            //{
            //    mapType = BlogConfiguration.GetBingStaticMapType(dr["MapType"].ToString());

            //    return "<img src='http://dev.virtualearth.net/REST/v1/Imagery/Map/"
            //    + mapType + "/"
            //    + Server.UrlEncode(location)
            //    + "?mapsize=" + mapWidth + "," + mapHeight
            //    + "&amp;zoomLevel=" + mapZoom
            //    + "&amp;key=" + WebConfigSettings.BingMapsApiKey
            //    + "' />"
            //    ;

            //}

            if (siteSettings.GmapApiKey.Length == 0) { return string.Empty; }

            mapType = BlogConfiguration.GetGoogleStaticMapType(dr["MapType"].ToString());


            return "<img src='http://maps.googleapis.com/maps/api/staticmap?center="
                + Server.UrlEncode(location)
                + "&amp;zoom=" + mapZoom
                + "&amp;size=" + mapWidth + "x" + mapHeight
                + "&amp;maptype=" + mapType
                + "&amp;key=" + siteSettings.GmapApiKey
                + "&amp;sensor=false"
                + "' />"
                ;
        }

        private int maxTweetLength = 140;

        private string GenerateTweetThisLink(string titleToTweet, string urlToTweet)
        {
            string format = "<a class='tweetthislink' title='Tweet This' href='{0}'><img src='" + imageSiteRoot + "/Data/SiteImages/tweet-button-2015.png' alt='Tweet This' /></a>";

            string twitterUrl;

            int maxTitleLength = maxTweetLength - (urlToTweet.Length + 1);
            if (maxTitleLength > 3)
            {
                if ((titleToTweet.Length > maxTitleLength) && (titleToTweet.Length > 3))
                {
                    titleToTweet = titleToTweet.Substring(0, (maxTitleLength - 3)) + "...";
                }

                twitterUrl = "http://twitter.com/home?status=" + Page.Server.UrlEncode(titleToTweet + " " + urlToTweet);

            }
            else
            {
                twitterUrl = "http://twitter.com/home?status=" + Page.Server.UrlEncode(urlToTweet);
            }

            return string.Format(CultureInfo.InvariantCulture, format, twitterUrl);
        }

        private string GenerateFacebookLikeButton(string urlToLike)
        {
            string format = "<div class='fblikebutton'><iframe src='http://www.facebook.com/plugins/like.php?href={0}&amp;layout=standard&amp;show_faces=false&amp;width=450&amp;height=35&amp;action=like&amp;colorscheme=light' scrolling='no' frameborder='0' allowTransparency='true' style='border:none; overflow:hidden;width:450px; height:35px;'></iframe></div>";

            return string.Format(CultureInfo.InvariantCulture, format, Page.Server.UrlEncode(urlToLike));
        }

        private void LoadSettings()
        {
            pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            categoryId = WebUtils.ParseInt32FromQueryString("cat", categoryId);
            siteSettings = CacheHelper.GetCurrentSiteSettings();

            // newer implementation combines params as p=pageid~moduleid~categoryid
            string f = WebUtils.ParseStringFromQueryString("p", string.Empty);
            if ((f.Length > 0) && (f.Contains("~")))
            {
                List<string> parms = f.SplitOnCharAndTrim('~');

                if (parms.Count >= 1)
                {
                    int.TryParse(parms[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out pageID);
                }

                if (parms.Count >= 2)
                {
                    int.TryParse(parms[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out moduleId);
                }

                if (parms.Count >= 3)
                {
                    int.TryParse(parms[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out categoryId);
                }

                
            }
            
            
            securityBypassGuid = WebUtils.ParseGuidFromQueryString("g", securityBypassGuid);
            attachmentBaseUrl = SiteUtils.GetFileAttachmentUploadPath();
            pageSettings = CacheHelper.GetPage(pageID);
            module = GetModule();

            if ((moduleId == -1) || (module == null)) { return; }

            bool bypassPageSecurity = false;

            if ((securityBypassGuid != Guid.Empty) && (securityBypassGuid == WebConfigSettings.InternalFeedSecurityBypassKey))
            {
                bypassPageSecurity = true;
            }

            if (
                (bypassPageSecurity)
                || (WebUser.IsInRoles(pageSettings.AuthorizedRoles))
                || (WebUser.IsInRoles(module.ViewRoles))
                )
            {
                canView = true;
            }

            if (!canView) { return; }

            if (WebConfigSettings.UseFolderBasedMultiTenants)
            {
                navigationSiteRoot = SiteUtils.GetNavigationSiteRoot();
                blogBaseUrl = navigationSiteRoot;
                imageSiteRoot = WebUtils.GetSiteRoot();
                cssBaseUrl = imageSiteRoot;
            }
            else
            {
                navigationSiteRoot = WebUtils.GetHostRoot();
                blogBaseUrl = SiteUtils.GetNavigationSiteRoot();
                imageSiteRoot = navigationSiteRoot;
                cssBaseUrl = WebUtils.GetSiteRoot();

            }

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new BlogConfiguration(moduleSettings);

            if (config.FeedIsDisabled) { canView = false; }

            if ((config.FeedburnerFeedUrl.Length > 0) && (config.FeedburnerFeedUrl.StartsWith("http")) && (BlogConfiguration.UseRedirectForFeedburner))
            {
                shouldRedirectToFeedburner = true;
                if ((Request.UserAgent != null) && (Request.UserAgent.Contains("FeedBurner")))
                {
                    shouldRedirectToFeedburner = false; // don't redirect if the feedburner bot is reading the feed
                }

                Guid redirectBypassToken = WebUtils.ParseGuidFromQueryString("r", Guid.Empty);
                if (redirectBypassToken == Global.FeedRedirectBypassToken)
                {
                    shouldRedirectToFeedburner = false; // allows time for user to subscribe to autodiscovery links without redirecting
                }

            }
        }




        private string FormatBlogUrl(string itemUrl, int itemId)
        {
            //if (itemUrl.Length > 0)
            //    return blogBaseUrl + itemUrl.Replace("~", string.Empty);
            if ((WebConfigSettings.UseUrlReWriting) && (itemUrl.Length > 0))
            {
                return WebUtils.ResolveServerUrl(blogBaseUrl + itemUrl.Replace("~", string.Empty));
            }

            return blogBaseUrl + "/Blog/ViewPost.aspx?pageid=" + pageID.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();

        }

        private Module GetModule()
        {

            Module m = null;
            if (pageSettings != null)
            {
                foreach (Module module in pageSettings.Modules)
                {
                    if (module.ModuleId == moduleId)
                        m = module;
                }
            }

            if (m == null) return null;
            if (m.ModuleId == -1) return null;

            if (m.ControlSource.ToLower().Contains("blogmodule.ascx"))
            {
                return m;
            }

            return null;
        }



        private void RenderError(string message)
        {

            Response.Write(message);
            Response.End();
        }





    }
}
