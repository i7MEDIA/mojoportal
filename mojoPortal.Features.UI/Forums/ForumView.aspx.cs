/// Author:				
/// Created:			2004-09-12
/// Last Modified:	    2012-06-18

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.ForumUI
{
    public partial class ForumView : mojoBasePage
	{
        protected int PageId = -1;
        protected int ModuleId = -1;
        protected int ItemId = -1;
        private int pageNumber = 1;
        private Hashtable moduleSettings;
        private bool userCanEdit = false;
        protected ForumConfiguration config = null;
        private Forum forum = null;
        private ForumParameterParser forumParams = null;
        

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


        private void Page_Load(object sender, EventArgs e)
		{
            if (Page.IsPostBack) return;

            if ((siteSettings != null)&&(CurrentPage != null))
            {
                if ((SiteUtils.SslIsAvailable())
                    &&((siteSettings.UseSslOnAllPages)||(CurrentPage.RequireSsl))
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

            if ((forum == null)||(!forumParams.ParamsAreValid))
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            // if we get here then the module exists on the page and matches the forum module id
            // so check view permission
            if (!UserCanViewPage(ModuleId, Forum.FeatureGuid))
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this);
                    return;
                }
                else
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }
                
            }
            //this page has no content other than links
            SiteUtils.AddNoIndexFollowMeta(Page);

            LoadSettings();

            if((!forum.Visible)&&(!userCanEdit))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
           
            PopulateLabels();
			PopulateControls();

            AnalyticsSection = ConfigHelper.GetStringProperty("AnalyticsForumSection", "forums");


            LoadSideContent(config.ShowLeftContent, config.ShowRightContent);
            LoadAltContent(ForumConfiguration.ShowTopContent, ForumConfiguration.ShowBottomContent);

		}

		

		private void PopulateControls()
		{
            if (forum == null) { return; }

            Title = SiteUtils.FormatPageTitle(siteSettings, FormatTitle(forum));

            heading.Text = forum.Title;
			litForumDescription.Text = forum.Description;
            fgpDescription.Visible = (forum.Description.Length > 0) && !displaySettings.ForumViewHideForumDescription;

            MetaDescription = string.Format(CultureInfo.InvariantCulture, ForumResources.ForumMetaDescriptionFormat, FormatTitle(forum));

            if (Page.Header != null)
            {

                Literal link = new Literal();
                link.ID = "forumurl";

                string canonicalUrl;
                if (ForumConfiguration.CombineUrlParams)
                {
                    canonicalUrl = SiteRoot
                        + "/Forums/ForumView.aspx?pageid=" + PageId.ToInvariantString()
                        + "&amp;f=" + ForumParameterParser.FormatCombinedParam(forum.ItemId, pageNumber);
                   
                }
                else
                {
                    canonicalUrl = SiteRoot
                        + "/Forums/ForumView.aspx?"
                        + "ItemID=" + forum.ItemId.ToInvariantString()
                        + "&amp;mid=" + ModuleId.ToInvariantString()
                        + "&amp;pageid=" + PageId.ToInvariantString()
                        + "&amp;pagenumber=" + pageNumber.ToInvariantString();
                }

                string nextUrl;
                if (ForumConfiguration.CombineUrlParams)
                {
                    nextUrl = SiteRoot
                        + "/Forums/ForumView.aspx?pageid=" + PageId.ToInvariantString()
                        + "&amp;f=" + ForumParameterParser.FormatCombinedParam(forum.ItemId, pageNumber + 1);
                }
                else
                {
                    nextUrl = SiteRoot
                                + "/Forums/ForumView.aspx?"
                                + "ItemID=" + forum.ItemId.ToInvariantString()
                                + "&amp;mid=" + ModuleId.ToInvariantString()
                                + "&amp;pageid=" + PageId.ToInvariantString()
                                + "&amp;pagenumber=" + (pageNumber + 1).ToInvariantString();
                }

                string previousUrl;
                if (ForumConfiguration.CombineUrlParams)
                {
                    previousUrl = SiteRoot
                        + "/Forums/ForumView.aspx?pageid=" + PageId.ToInvariantString()
                        + "&amp;f=" + ForumParameterParser.FormatCombinedParam(forum.ItemId, pageNumber - 1);
                }
                else
                {
                    previousUrl = SiteRoot
                                + "/Forums/ForumView.aspx?"
                                + "ItemID=" + forum.ItemId.ToInvariantString()
                                + "&amp;mid=" + ModuleId.ToInvariantString()
                                + "&amp;pageid=" + PageId.ToInvariantString()
                                + "&amp;pagenumber=" + (pageNumber - 1).ToInvariantString();
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

                link.Text = "\n<link rel='canonical' href='" + canonicalUrl + "' />";

                Page.Header.Controls.Add(link);

                Literal nextLink = new Literal();
                nextLink.ID = "forumNextLink";
                nextLink.Text = "\n<link rel='next' href='" + nextUrl + "' />";

                Literal prevLink = new Literal();
                prevLink.ID = "forumPrevLink";
                prevLink.Text = "\n<link rel='prev' href='" + previousUrl + "' />";

                if (forum.TotalPages > 1)
                {
                    if (pageNumber == 1) // first page
                    {
                        Page.Header.Controls.Add(nextLink);
                    }
                    else if (pageNumber == forum.TotalPages) // last page
                    {
                        Page.Header.Controls.Add(prevLink);
                    }
                    else //other pages
                    {
                        Page.Header.Controls.Add(prevLink);
                        Page.Header.Controls.Add(nextLink);
                    }

                }
            }

           
		}

        private string FormatTitle(Forum forum)
        {
            if (forum.TotalPages > 1)
            {
                return string.Format(CultureInfo.InvariantCulture,
                    ForumResources.ThreadPageTitleFormat,
                    forum.Title, pageNumber.ToInvariantString());
            }

            return forum.Title;
        }


        private void PopulateLabels()
        {
            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            
        }

        private void LoadParams()
        {
            forumParams = new ForumParameterParser(this);
            forumParams.Parse();

            PageId = forumParams.PageId;
            ModuleId = forumParams.ModuleId;
            ItemId = forumParams.ItemId;
            pageNumber = forumParams.PageNumber;

            forum = forumParams.Forum;
        }
        

        private void LoadSettings()
        {
            
           
            userCanEdit = UserCanEditModule(ModuleId);

            

            moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);

            config = new ForumConfiguration(moduleSettings);
            searchBoxTop.Visible = config.ShowForumSearchBox && !displaySettings.HideSearchOnForumView;

            if ((searchBoxTop.Visible) && (displaySettings.UseBottomSearchOnForumView))
            {
                searchBoxTop.Visible = false;
                searchBoxBottom.Visible = true;
            }

            threadList.Forum = forum;
            threadList.Config = config;
            threadList.PageId = PageId;
            threadList.ModuleId = ModuleId;
            threadList.ItemId = ItemId;
            threadList.PageNumber = pageNumber;
            threadList.SiteRoot = SiteRoot;
            threadList.NonSslSiteRoot = SiteUtils.GetInSecureNavigationSiteRoot();
            threadList.ImageSiteRoot = ImageSiteRoot;
            threadList.IsEditable = userCanEdit;

            threadListAlt.Forum = forum;
            threadListAlt.Config = config;
            threadListAlt.PageId = PageId;
            threadListAlt.ModuleId = ModuleId;
            threadListAlt.ItemId = ItemId;
            threadListAlt.PageNumber = pageNumber;
            threadListAlt.SiteRoot = SiteRoot;
            threadListAlt.NonSslSiteRoot = threadList.NonSslSiteRoot;
            threadListAlt.ImageSiteRoot = ImageSiteRoot;
            threadListAlt.IsEditable = userCanEdit;

            if (displaySettings.UseAltThreadList)
            {
                threadList.Visible = false;
                threadListAlt.Visible = true;
            }

            if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

            AddClassToBody("forumview");

            if ((CurrentPage != null)&&(CurrentPage.BodyCssClass.Length > 0))
            {
                AddClassToBody(CurrentPage.BodyCssClass);
            }

            if (ForumConfiguration.TrackFakeTopicUrlInAnalytics) { SetupAnalytics(); }

        }

        private void SetupAnalytics()
        {
            if (siteSettings.GoogleAnalyticsAccountCode.Length == 0) { return; }
            if (forum == null) { return; }
            if (forum.Title.Length == 0) { return; }

            AnalyticsAsyncTopScript asyncAnalytics = Page.Master.FindControl("analyticsTop") as AnalyticsAsyncTopScript;
            if (asyncAnalytics != null)
            {
                string urlToTrack = ForumConfiguration.FakeTrackingBaseUrl + SiteUtils.SuggestFriendlyUrl(forum.Title, siteSettings);
                asyncAnalytics.PageToTrack = urlToTrack;
            }

        }

       
	}
}
