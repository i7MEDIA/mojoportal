// Author:					
// Created:				    2004-09-19
// Last Modified:			2014-07-14

using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using log4net;

namespace mojoPortal.Web.ForumUI
{
    public partial class ForumThreadView : mojoBasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ForumThreadView));
        private static bool debugLog = log.IsDebugEnabled;
       
        protected int PageId = -1;
		protected int moduleId = -1;
        private int threadId = 0;
        protected int ItemId = -1;
        protected bool IsAdmin = false;
        protected bool IsEditable = false;
        protected int PageNumber = 1;
        protected bool filterContentFromTrustedUsers = false;
        protected CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        private Hashtable moduleSettings;
        protected ForumConfiguration config = null;
        private SiteUser currentUser = null;
        private Forum forum = null;
        private ForumThread thread = null;
        private ThreadParameterParser threadParams = null;

        
        private void Page_Load(object sender, EventArgs e)
		{
            if ((siteSettings != null) && (CurrentPage != null))
            {
                if ((SiteUtils.SslIsAvailable())
                    && ((siteSettings.UseSslOnAllPages) || (CurrentPage.RequireSsl))
                    )
                {
                    SiteUtils.ForceSsl();
                }
                else
                {
                    SiteUtils.ClearSsl();
                }

            }

            LoadParams();

            if (!threadParams.ParamsAreValid)
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            if (!UserCanViewPage(moduleId, Forum.FeatureGuid))
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this);
                }
                else
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                }
                return;
            }

            LoadSettings();
            PopulateLabels();

            if (Page.IsPostBack) return;

            AddConnoicalUrl();
			PopulateControls();

            if (UserCanEditModule(moduleId, Forum.FeatureGuid))
            {
                heading.LiteralExtraMarkup = "&nbsp;<a href='"
                    + SiteRoot
                    + "/Forums/EditThread.aspx?pageid=" + PageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;thread=" + threadId.ToInvariantString()
                    + "' class='ModuleEditLink'>" + ForumResources.ForumThreadEditLabel + "</a>";
            }

            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsForumSection", "forums");

            LoadSideContent(config.ShowLeftContent, config.ShowRightContent);
            LoadAltContent(ForumConfiguration.ShowTopContent, ForumConfiguration.ShowBottomContent);
		}

        
		private void PopulateControls()
		{
            if (forum == null) { return; }
            if (thread == null) { return; }

            if (ForumConfiguration.UsePageNameInThreadTitle)
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName + " - " + SecurityHelper.RemoveMarkup(FormatTitle(thread)));
            }
            else
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, SecurityHelper.RemoveMarkup(FormatTitle(thread)));
            }

            if (thread.PageTitleOverride.Length > 0)
            {
                Title = thread.PageTitleOverride;
            }

            if (thread.SetNoIndexMeta)
            {
                SiteUtils.AddNoIndexMeta(this);
            }

            litForumDescription.Text = forum.Description;
            fgpDescription.Visible = (forum.Description.Length > 0) && !displaySettings.HideForumDescriptionOnPostList;

            // google does not use meta description in page rankings
            //http://googlewebmastercentral.blogspot.com/2009/09/google-does-not-use-keywords-meta-tag.html

            // big forum sites like stackoverflow don't use meta descrition on posts
            //http://webmasters.stackexchange.com/questions/1721/why-dont-websites-have-a-description-meta-tag-in-the-head-section
            // if you view the source of a post page on http://forums.asp.net/ you will also see no meta description
            // therefore as of 2012-06-18 I've added a setting to disable it by default but to allow it to be used for those who may disagree
            if (ForumConfiguration.UseMetaDescriptionOnThreads)
            {
                MetaDescription = string.Format(
                    CultureInfo.InvariantCulture, 
                    ForumResources.ForumThreadMetaDescriptionFormat, 
                    SecurityHelper.RemoveMarkup(thread.Subject));
            }

            if (ForumConfiguration.CombineUrlParams)
            {
                lnkForum.HRef = SiteRoot + "/Forums/ForumView.aspx?pageid=" + PageId.ToInvariantString()
                    + "&amp;f=" + forum.ItemId.ToInvariantString() + "~1" ;
            }
            else
            {
                lnkForum.HRef = SiteRoot + "/Forums/ForumView.aspx?ItemID="
                    + forum.ItemId.ToInvariantString()
                    + "&amp;pageid=" + PageId.ToInvariantString()
                    + "&amp;mid=" + forum.ModuleId.ToInvariantString();
            }

			lnkForum.InnerHtml = forum.Title;

            if (displaySettings.HideHeadingOnThreadView)
            {
                heading.Visible = false;
            }
            else
            {
                heading.Text = Server.HtmlEncode(thread.Subject);
            }
            if (!displaySettings.HideCurrentCrumbOnThreadcrumbs)
            {
                litThreadDescription.Text = Server.HtmlEncode(thread.Subject);
            }
			
           
		}

        private void AddConnoicalUrl()
        {
            if (Page.Header == null) { return; }
            if (ForumConfiguration.DisableThreadCanonicalUrl) { return; }


            string canonicalUrl;
            string nextUrl;
            string previousUrl;

            if (ForumConfiguration.CombineUrlParams)
            {
                canonicalUrl = SiteRoot
                    + "/Forums/Thread.aspx?pageid="
                    + PageId.ToInvariantString()
                    + "&amp;t=" + threadId.ToInvariantString()
                    + "~-1"; //-1 is the view all which google prefers over paged views

                nextUrl = SiteRoot
                    + "/Forums/Thread.aspx?pageid="
                    + PageId.ToInvariantString()
                    + "&amp;t=" + threadId.ToInvariantString()
                    + "~" + (PageNumber + 1).ToInvariantString();

                previousUrl = SiteRoot
                    + "/Forums/Thread.aspx?pageid="
                    + PageId.ToInvariantString()
                    + "&amp;t=" + threadId.ToInvariantString()
                    + "~" + (PageNumber - 1).ToInvariantString();
            }
            else
            {
                canonicalUrl = SiteRoot
                    + "/Forums/Thread.aspx?pageid="
                    + PageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "&amp;ItemID=" + ItemId.ToInvariantString()
                    + "&amp;thread=" + threadId.ToInvariantString()
                    + "&amp;pagenumber=-1";

                nextUrl = SiteRoot
                + "/Forums/Thread.aspx?pageid="
                + PageId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString()
                + "&amp;ItemID=" + ItemId.ToInvariantString()
                + "&amp;thread=" + threadId.ToInvariantString()
                + "&amp;pagenumber=" + (PageNumber + 1).ToInvariantString();

                previousUrl = SiteRoot
                + "/Forums/Thread.aspx?pageid="
                + PageId.ToInvariantString()
                + "&amp;mid=" + moduleId.ToInvariantString()
                + "&amp;ItemID=" + ItemId.ToInvariantString()
                + "&amp;thread=" + threadId.ToInvariantString()
                + "&amp;pagenumber=" + (PageNumber - 1).ToInvariantString();
            }

            
            if (SiteUtils.IsSecureRequest() && (!CurrentPage.RequireSsl) && (!siteSettings.UseSslOnAllPages))
            {
                if (WebConfigSettings.ForceHttpForCanonicalUrlsThatDontRequireSsl)
                {
                    canonicalUrl = canonicalUrl.Replace("https:", "http:");
                    nextUrl = nextUrl.Replace("https:", "http:");
                    previousUrl = previousUrl.Replace("https:", "http:");
                }

            }

            Literal link = new Literal();
            link.ID = "threadurl";
            link.Text = "\n<link rel='canonical' href='" + canonicalUrl + "' />";

            Page.Header.Controls.Add(link);

            Literal nextLink = new Literal();
            nextLink.ID = "threadNextLink";
            nextLink.Text = "\n<link rel='next' href='" + nextUrl + "' />";

            Literal prevLink = new Literal();
            prevLink.ID = "threadPrevLink";
            prevLink.Text = "\n<link rel='prev' href='" + previousUrl + "' />";

            if (thread.TotalPages > 1)
            {
                if (PageNumber == 1) // first page
                {
                    Page.Header.Controls.Add(nextLink);
                }
                else if ((PageNumber != -1) &&(PageNumber == thread.TotalPages)) // last page
                {
                    Page.Header.Controls.Add(prevLink);
                }
                else if (PageNumber != -1) //other pages -1 is the view all view
                {
                    Page.Header.Controls.Add(prevLink);
                    Page.Header.Controls.Add(nextLink);
                }

            }
            

        }

        private string FormatTitle(ForumThread thread)
        {
            if (thread.TotalPages > 1)
            {
                return string.Format(CultureInfo.InvariantCulture,
                    ForumResources.ThreadPageTitleFormat,
                    thread.Subject, PageNumber.ToInvariantString());
            }

            return thread.Subject;
        }

        private void PopulateLabels()
        {
            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

        }

        private void LoadSettings()
        {
            thread = threadParams.Thread;


            if (thread.ThreadId == -1)
            {
                //thread does not exist, probably just got deleted
                //redirect back to thread list
                string redirectUrl;
                if (ForumConfiguration.CombineUrlParams)
                {
                    redirectUrl = SiteRoot + "/Forums/ForumView.aspx?pageid=" + PageId.ToInvariantString()
                    + "&f=" + ItemId.ToInvariantString() + "~1";
                }
                else
                {
                    redirectUrl = SiteRoot + "/Forums/ForumView.aspx?ItemID="
                    + ItemId.ToInvariantString()
                    + "&pageid=" + PageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString();
                }


                WebUtils.SetupRedirect(this, redirectUrl);

                return;

            }

            if (thread.ModuleId != moduleId)
            {
                //SiteUtils.RedirectToAccessDeniedPage(this);
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            forum = threadParams.Forum;

            //if (forum.ModuleId != moduleId)
            //{
            //    //SiteUtils.RedirectToAccessDeniedPage(this);
            //    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
            //    return;
            //}

            postList.Forum = forum;
            postList.Thread = thread;

            postListAlt.Forum = forum;
            postListAlt.Thread = thread;

            if (ForumConfiguration.TrackFakeTopicUrlInAnalytics) { SetupAnalytics(); }

            if ((IsEditable)||(WebUser.IsInRoles(forum.RolesThatCanModerate)))
            {
                SetupNotifyScript();
            }
        }

        private void SetupAnalytics()
        {
            if (siteSettings.GoogleAnalyticsAccountCode.Length == 0)  { return; }
            if (thread == null) { return; }
            if (thread.Subject.Length == 0) { return; }

            AnalyticsAsyncTopScript asyncAnalytics = Page.Master.FindControl("analyticsTop") as AnalyticsAsyncTopScript;
            if (asyncAnalytics != null)
            {
                string urlToTrack = ForumConfiguration.FakeTrackingBaseUrl + SiteUtils.SuggestFriendlyUrl(thread.Subject, siteSettings);
                asyncAnalytics.PageToTrack = urlToTrack;
            }

        }


        private void LoadParams()
        {
            PageId = WebUtils.ParseInt32FromQueryString("pageid", PageId);
            //moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            //ItemId = WebUtils.ParseInt32FromQueryString("ItemID", ItemId);
            //threadId = WebUtils.ParseInt32FromQueryString("thread", threadId);
            //PageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", PageNumber);

            threadParams = new ThreadParameterParser(this);
            threadParams.Parse();

            moduleId = threadParams.ModuleId;
            ItemId = threadParams.ItemId;
            threadId = threadParams.ThreadId;
            PageNumber = threadParams.PageNumber;
            
            
            IsAdmin = WebUser.IsAdmin;
            IsEditable = UserCanEditModule(moduleId, Forum.FeatureGuid);
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new ForumConfiguration(moduleSettings);

            postList.Config = config;
            postList.PageId = PageId;
            postList.ModuleId = moduleId;
            postList.ItemId = ItemId;
            postList.ThreadId = threadId;
            postList.PageNumber = PageNumber;
            postList.IsAdmin = IsAdmin;
            postList.IsCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);
            postList.SiteRoot = SiteRoot;
            postList.ImageSiteRoot = ImageSiteRoot;
            postList.SiteSettings = siteSettings;
            postList.IsEditable = IsEditable;

            postListAlt.Config = config;
            postListAlt.PageId = PageId;
            postListAlt.ModuleId = moduleId;
            postListAlt.ItemId = ItemId;
            postListAlt.ThreadId = threadId;
            postListAlt.PageNumber = PageNumber;
            postListAlt.IsAdmin = IsAdmin;
            postListAlt.IsCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);
            postListAlt.SiteRoot = SiteRoot;
            postListAlt.ImageSiteRoot = ImageSiteRoot;
            postListAlt.SiteSettings = siteSettings;
            postListAlt.IsEditable = IsEditable;
            
            if (Request.IsAuthenticated)
            {
                if (currentUser == null) { currentUser = SiteUtils.GetCurrentSiteUser(); }

                if ((currentUser != null) && (ItemId > -1))
                {
                    postList.UserId = currentUser.UserId;
                    postList.IsSubscribedToForum = Forum.IsSubscribed(ItemId, currentUser.UserId);

                    postListAlt.UserId = currentUser.UserId;
                    postListAlt.IsSubscribedToForum = postList.IsSubscribedToForum;
                   
                }

            }

            if (displaySettings.UseAltPostList)
            {
                postList.Visible = false;
                postListAlt.Visible = true;
            }

            if (displaySettings.OverrideThreadHeadingElement.Length > 0)
            {
                heading.HeadingTag = displaySettings.OverrideThreadHeadingElement;
            }

            AddClassToBody("forumthread");

            if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

            if ((CurrentPage != null) && (CurrentPage.BodyCssClass.Length > 0))
            {
                AddClassToBody(CurrentPage.BodyCssClass);
            }

           

        }

        private void SetupNotifyScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("$(document).ready(function() {");
            script.Append("$('.forumcommand').each(function(index) {");
            
            script.Append("$(this).on('click', function(e){");
            // stop the click from triggerring anything else, ie navigation
            script.Append("e.preventDefault(); e.stopPropagation(); ");

            script.Append("var d = {};");
            script.Append("d.postId = $(this).data('post'); ");
            script.Append("d.cmd = $(this).data('cmd'); ");
            script.Append("d.threadId = " + threadId.ToInvariantString() + ";");
            script.Append("d.pageId = " + PageId.ToInvariantString() + ";");
            script.Append("d.moduleId = " + moduleId.ToInvariantString() + ";");
            script.Append("d.pageNumber = " + PageNumber.ToInvariantString() + ";");

            script.Append("var prompt =\"" + HttpUtility.HtmlAttributeEncode(ForumResources.ConfirmSendNotification) + "\";");
            script.Append("if(d.cmd == \"marksent\") {");
            script.Append("prompt =\"" + HttpUtility.HtmlAttributeEncode(ForumResources.ConfirmMarkAsSent) + "\";");
            script.Append("} ");

            script.Append("if (confirm(prompt)) {");
            script.Append("$.ajax({");
            script.Append(" type: \"POST\"");
#if NET35 || NET40
            script.Append(",url: \"" + SiteUtils.GetNavigationSiteRoot() + "/Forums/Mod.ashx\"");
#else
            // .NET 4.5 can use web api
            script.Append(",url: \"" + SiteUtils.GetNavigationSiteRoot() + "/api/forummod\"");
            
#endif

            script.Append(",data: d");
            //script.Append(",contentType: \"application/json; charset=utf-8\"");
            script.Append(",dataType: \"json\"");
            script.Append(",error: function(jqXHR,textStatus,errorThrown) {");
            script.Append("alert(errorThrown); ");
            script.Append("}"); //end error
            script.Append(",beforeSend: function() {");
            //script.Append("alert('before send ' + d.postId);");
            script.Append("$(\"a[data-post='\" + d.postId + \"']\").hide();");
            script.Append("$(\"div[data-tb='\" + d.postId + \"']\").progressbar({value:false});");
            //script.Append("alert('before send ' + d.postId);");

            script.Append("}"); //end beforeSend

            script.Append(",success: function(data) {");
            
            script.Append("$(\"div[data-tb='\" + d.postId + \"']\").progressbar(\"destroy\");");
            script.Append("if(data.msg == 'success') {");
            //script.Append("$(\"a[data-post='\" + d.postId + \"'\").hide();");
            script.Append("$(\"div[data-tb='\" + d.postId + \"']\").hide();");
            script.Append("} else {");
            script.Append("alert(data.msg);");
            script.Append("$(\"a[data-post='\" + d.postId + \"']\").show();");
            script.Append("}"); //end if
            
            script.Append("}"); //end success

            script.Append("});"); //end ajax
            script.Append("}"); // end confirm

            script.Append("});");
            //script.Append("alert('wired');");

            script.Append("});"); 
            script.Append("});");  //end $(document).ready

            ScriptManager.RegisterStartupScript(this, typeof(Page),
                   "thread-notify", "\n<script type=\"text/javascript\" >"
                   + script.ToString() + "</script>", false);
        }

        



        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion

	}
}
