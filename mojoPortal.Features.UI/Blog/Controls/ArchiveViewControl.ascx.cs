// Author:				
// Created:			    2004-08-14
// Last Modified:	    2012-04-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

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

namespace mojoPortal.Web.BlogUI
{
   
    /// <summary>
    /// this control is no longer used as of 2012-04-16
    /// the PostList control has been modifued to support all the needed list views
    /// </summary>
    public partial class ArchiveViewControl : UserControl
    {
       
        protected int PageId = -1;
        protected int ModuleId = 0;
        protected int ItemId = 0;
        protected int Month = DateTime.UtcNow.Month;
        protected int Year = DateTime.UtcNow.Year;
        //private int countOfDrafts = 0;
        //private Hashtable moduleSettings;
        private mojoBasePage basePage;
        private Module blogModule = null;
        protected string FeedBackLabel = ConfigurationManager.AppSettings["BlogFeedbackLabel"];
        protected Double TimeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private string DisqusSiteShortName = string.Empty;
        protected string SiteRoot = string.Empty;
        protected string ImageSiteRoot = string.Empty;
        private SiteSettings siteSettings = null;
        protected BlogConfiguration config = new BlogConfiguration();
        private bool useFriendlyUrls = true;
        protected bool IsEditable = false;
        protected bool AllowComments = false;

        public BlogConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);

            basePage = Page as mojoBasePage;
            SiteRoot = basePage.SiteRoot;
            ImageSiteRoot = basePage.ImageSiteRoot;
        }
        #endregion

        private void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (
                (basePage == null)
                || (!basePage.UserCanViewPage(ModuleId, Blog.FeatureGuid))
            )
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            
            
            LoadSettings();
            if (!config.NavigationOnRight)
            {
                this.divblog.CssClass = "blogcenter-leftnav";

            }
            PopulateLabels();
            AddConnoicalUrl();

            if (!IsPostBack)
            {
                
                //if ((Month > -1) && (Year > -1))
                //{
                    //DateTime selectedMonth = DateTime.Now;
                    DateTime selectedMonth = new DateTime(Year, Month, 1, CultureInfo.CurrentCulture.Calendar);
                    try
                    {
                       // selectedMonth = new DateTime(year, month, 1);
                        selectedMonth = new DateTime(Year, Month, 1, CultureInfo.CurrentCulture.Calendar);
                    }
                    catch (Exception)
                    { }

                    heading.Text = Page.Server.HtmlEncode(BlogResources.BlogArchivesPrefixLabel
                        + selectedMonth.ToString("MMMM, yyyy"));

                    if (blogModule != null)
                    {
                        basePage.Title = SiteUtils.FormatPageTitle(basePage.SiteInfo, blogModule.ModuleTitle + " - " + BlogResources.BlogArchivesPrefixLabel
                        + selectedMonth.ToString("MMMM, yyyy"));

                        basePage.MetaDescription = string.Format(CultureInfo.InvariantCulture, 
                            BlogResources.ArchiveMetaDescriptionFormat, 
                            blogModule.ModuleTitle, 
                            selectedMonth.ToString("MMMM, yyyy"));

                        
                    }

                  
                    using(IDataReader reader = Blog.GetBlogEntriesByMonth(Month, Year, ModuleId))
                    {
                         dlArchives.DataSource = reader;
                         dlArchives.DataBind();
                    }
                
                

               
            }

            basePage.LoadSideContent(config.ShowLeftContent, config.ShowRightContent);
            basePage.LoadAltContent(BlogConfiguration.ShowTopContent, BlogConfiguration.ShowBottomContent);

        }


        protected string FormatBlogUrl(string itemUrl, int itemId)
        {
            if (useFriendlyUrls && (itemUrl.Length > 0))
                return SiteRoot + itemUrl.Replace("~", string.Empty);

            return SiteRoot + "/Blog/ViewPost.aspx?pageid=" + PageId.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString();

        }

        

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            //moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);

            if (Request.IsAuthenticated)
            {
                if (basePage.UserCanEditModule(ModuleId, Blog.FeatureGuid))
                {
                    IsEditable = true;
                }
            }

            //config = new BlogConfiguration(moduleSettings);

            if (config.DisqusSiteShortName.Length > 0)
            {
                DisqusSiteShortName = config.DisqusSiteShortName;
            }
            else
            {
                DisqusSiteShortName = siteSettings.DisqusSiteShortName;
            }

            AllowComments = config.AllowComments && !displaySettings.ArchiveViewHideFeedbackLink;
            

            if ((DisqusSiteShortName.Length > 0) && (config.CommentSystem == "disqus")) 
            { 
                navTop.ShowCommentCount = false;
                navBottom.ShowCommentCount = false;
            }

            navTop.ModuleId = ModuleId;
            navTop.PageId = PageId;
            navTop.IsEditable = IsEditable;
            navTop.Config = config;
            navTop.SiteRoot = SiteRoot;
            navTop.ImageSiteRoot = ImageSiteRoot;

            navBottom.ModuleId = ModuleId;
            navBottom.PageId = PageId;
            navBottom.IsEditable = IsEditable;
            navBottom.Config = config;
            navBottom.SiteRoot = SiteRoot;
            navBottom.ImageSiteRoot = ImageSiteRoot;

            navTop.Visible = false;

            if (config.ShowArchives
                || config.ShowAddFeedLinks
                || config.ShowCategories
                || config.ShowFeedLinks
                || config.ShowStatistics
                || (config.UpperSidebar.Length > 0)
                || (config.LowerSidebar.Length > 0)
                )
            {
                navTop.Visible = true;
            }

            if (!navTop.Visible)
            {
                divblog.CssClass = "blogcenter-nonav";
            }

            navBottom.Visible = false;

            if ((navTop.Visible) && (displaySettings.UseBottomNavigation))
            {
                navTop.Visible = false;
                navBottom.Visible = true;
            }

            //countOfDrafts = Blog.CountOfDrafts(ModuleId);
            useFriendlyUrls = BlogConfiguration.UseFriendlyUrls(ModuleId);
            if (!WebConfigSettings.UseUrlReWriting) { useFriendlyUrls = false; }


        }

        protected string FormatBlogDate(DateTime startDate)
        {
            if (timeZone != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString(config.DateTimeFormat);

            }

            return startDate.AddHours(TimeOffset).ToString(config.DateTimeFormat);

        }


        private void PopulateLabels()
        {
           
            FeedBackLabel = BlogResources.BlogFeedbackLabel;
            
        }


        

        private void AddConnoicalUrl()
        {
            if (Page.Header == null) { return; }

            string canonicalUrl = SiteUtils.GetNavigationSiteRoot()
                + "/Blog/ViewArchive.aspx?month="
                + Month.ToInvariantString()
                + "&amp;year=" + Year.ToInvariantString()
                + "&amp;mid=" + ModuleId.ToInvariantString()
                + "&amp;pageid=" + PageId.ToInvariantString();


            if (SiteUtils.IsSecureRequest() && (!basePage.CurrentPage.RequireSsl) && (!siteSettings.UseSslOnAllPages))
            {
                if (WebConfigSettings.ForceHttpForCanonicalUrlsThatDontRequireSsl)
                {
                    canonicalUrl = canonicalUrl.Replace("https:", "http:");
                }

            }

            Literal link = new Literal();
            link.ID = "blogarchiveurl";
            link.Text = "\n<link rel='canonical' href='" + canonicalUrl + "' />";

            Page.Header.Controls.Add(link);

        }

        private void LoadParams()
        {
            TimeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            PageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            ModuleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            ItemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            Month = WebUtils.ParseInt32FromQueryString("month", Month);
            Year = WebUtils.ParseInt32FromQueryString("year", Year);

            // don't let the archive be used to see unpublished future posts
            try
            {
                //This line commentted by Asad Samarian 2009-01-08
                //DateTime d = new DateTime(year, month, 1,ResourceHelper.GetDefaultCulture().Calendar);
                //This line added by Asad Samarian 2009-01-08
                DateTime d = new DateTime(Year, Month, 1, CultureInfo.CurrentCulture.Calendar);
             
                if (d > DateTime.UtcNow)
                {
                    Month = DateTime.UtcNow.Month;
                    Year = DateTime.UtcNow.Year;

                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Month = DateTime.UtcNow.Month;
                Year = DateTime.UtcNow.Year;
            }

            blogModule = basePage.GetModule(ModuleId);

        }

    }
}
