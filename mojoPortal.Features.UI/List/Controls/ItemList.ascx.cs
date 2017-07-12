// Author:					
// Created:					2010-12-06
// Last Modified:			2015-01-19
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.LinksUI
{
    public partial class ItemList : UserControl
    {
        #region Properties

        protected string LinkImage = string.Empty;
        protected string DeleteImage = string.Empty;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
        private string linkCssClass = "mojolink";
        private int pageNumber = 1;
        private int totalPages = 1;
        private ListConfiguration config = new ListConfiguration();
        //private string cacheDependencyKey;
        private string webSnaprKey = string.Empty;

        public ListConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        private string siteRoot = string.Empty;
        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        private string imageSiteRoot = string.Empty;
        public string ImageSiteRoot
        {
            get { return imageSiteRoot; }
            set { imageSiteRoot = value; }
        }

        private bool isEditable = false;
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

        private int moduleId = -1;
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        private int pageId = -1;
        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            SetupScripts();

            if (Page.IsPostBack) { return; }
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (config.EnablePager)
            {
                using (IDataReader reader = Link.GetPage(ModuleId, pageNumber, config.PageSize, out totalPages))
                {
                    if (!config.UseAjaxPaging)
                    {
                        string pageUrl = SiteRoot + "/List/ViewList.aspx"
                           + "?pageid=" + pageId.ToInvariantString()
                           + "&amp;mid=" + moduleId.ToInvariantString()
                           + "&amp;pagenumber={0}";

                        pgr.PageURLFormat = pageUrl;
                        pgr.CurrentIndex = pageNumber;

                        if((Page is ViewListPage)&& (Page.Header != null))
                        {
                            string canonicalUrl = SiteRoot + "/List/ViewList.aspx"
                               + "?pageid=" + pageId.ToInvariantString()
                               + "&amp;mid=" + moduleId.ToInvariantString()
                               + "&amp;pagenumber=" + pageNumber.ToInvariantString();

                            string nextUrl = SiteRoot + "/List/ViewList.aspx"
                               + "?pageid=" + pageId.ToInvariantString()
                               + "&amp;mid=" + moduleId.ToInvariantString()
                               + "&amp;pagenumber=" + (pageNumber + 1).ToInvariantString();

                            string previousUrl = SiteRoot + "/List/ViewList.aspx"
                               + "?pageid=" + pageId.ToInvariantString()
                               + "&amp;mid=" + moduleId.ToInvariantString()
                               + "&amp;pagenumber=" + (pageNumber -1).ToInvariantString();

                            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                            PageSettings currentPage = CacheHelper.GetCurrentPage();

                            if (SiteUtils.IsSecureRequest() && (!currentPage.RequireSsl) && (!siteSettings.UseSslOnAllPages))
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

                            if (totalPages > 1)
                            {
                                if (pageNumber == 1) // first page
                                {
                                    Page.Header.Controls.Add(nextLink);
                                }
                                else if (pageNumber == totalPages) // last page
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

                    pgr.ShowFirstLast = true;
                    pgr.PageSize = config.PageSize;
                    pgr.PageCount = totalPages;

                    if (config.DescriptionOnly)
                    {
                        rptDescription.Visible = true;
                        rptLinks.Visible = false;
                        rptDescription.DataSource = reader;
                        rptDescription.DataBind();

                    }
                    else
                    {
                        rptDescription.Visible = false;
                        rptLinks.Visible = true;
                        rptLinks.DataSource = reader;
                        rptLinks.DataBind();

                    }

                    pgr.Visible = (totalPages > 1);

                }


            }
            else //!(config.EnablePager)
            {

                using (IDataReader reader = Link.GetLinks(ModuleId))
                {
                    if (config.DescriptionOnly)
                    {
                        rptDescription.Visible = true;
                        rptLinks.Visible = false;
                        rptDescription.DataSource = reader;
                        rptDescription.DataBind();

                    }
                    else
                    {
                        rptDescription.Visible = false;
                        rptLinks.Visible = true;
                        rptLinks.DataSource = reader;
                        rptLinks.DataBind();

                    }

                }
            }

        }

        void pgr_Command(object sender, CommandEventArgs e)
        {
            pageNumber = Convert.ToInt32(e.CommandArgument);
            pgr.CurrentIndex = pageNumber;
            PopulateControls();
            updPnl.Update();
        }

        void rptLinks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if ((e.CommandSource is Button) && (e.CommandName.Equals("delete")))
            {
                int itemId = Convert.ToInt32(e.CommandArgument);
                Link link = new Link(itemId);
                link.ContentChanged += new ContentChangedEventHandler(link_ContentChanged);
                link.Delete();

                //CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                CacheHelper.ClearModuleCache(link.ModuleId);

                WebUtils.SetupRedirect(this, Page.Request.RawUrl);

            }
        }

        void link_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["LinksIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }


        void rptLinks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Button btnDelete = e.Item.FindControl("btnDelete") as Button;
            UIHelper.AddConfirmationDialog(btnDelete, LinkResources.LinksDeleteLinkWarning);
        }


        protected string CreateLink(
            string title,
            string url,
            string description,
            string target)
        {
            if (string.IsNullOrEmpty(url)) { return string.Empty; }
            string webSnapr = string.Empty;

            if ((config.AddWebSnaprCssToLinks) && (webSnaprKey.Length > 0) && (!config.UseAjaxPaging))
            {
                webSnapr = "\n<div class='webSnapr'><script type=\"text/javascript\">wsr_snapshot('" + GetLinkUrl(url) + "', '" + webSnaprKey + "', 's');</script></div>";
            }

            String link =  "<a class='" + linkCssClass + "' href='" + GetLinkUrl(url) + "' "
                + GetTarget(target)
                + GetTitle(title, description)
                + ">"
                + title
                + "</a>" + webSnapr;

            return link;
        }

        private string GetTitle(String title, String description)
        {
            if ((title != null) && (title.Length > 0))
            {
                return " title='" + Server.HtmlEncode(title) + "' ";
            }

            return String.Empty;
        }

        private string GetTarget(string target)
        {
            if (
                (target != null)
                && (target == "_blank")
              )
            {
                return " target='_blank' ";
            }

            return string.Empty;

        }


        protected String GetLinkUrl(String dbFormatUrl)
        {
            if (dbFormatUrl.StartsWith("~/"))
            {
                return Page.ResolveUrl(ImageSiteRoot + dbFormatUrl.Replace("~/", "/"));
            }

            if (dbFormatUrl.StartsWith("/"))
            {
                return Page.ResolveUrl(ImageSiteRoot + dbFormatUrl);
            }


            return dbFormatUrl;

        }

        protected string FormatEditUrl(int itemId)
        {
            return SiteRoot + "/List/Edit.aspx?ItemID=" + itemId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString()
                + "&pageid=" + PageId.ToInvariantString();
        }

        private void SetupScripts()
        {
            if (config.UseAjaxPaging) { return; } //doesn't work except on first page when using ajax paging
            if (!config.AddWebSnaprCssToLinks) { return; }
            if (webSnaprKey.Length == 0) { return; }

            // this script doesn't support https as far as I know
            if (SiteUtils.IsSecureRequest()) { return; }

            
            mojoBasePage basePage = Page as mojoBasePage;
            if (basePage != null) { basePage.ScriptConfig.IncludeWebSnapr = true; }
            

           //ScriptManager.RegisterClientScriptBlock(
           //    this,
           //    typeof(System.Web.UI.Page),
           //     "websnapr", "\n<script src=\"http://bubble.websnapr.com/"
           //     + config.WebSnaprKey + "/swh/" + "\" type=\"text/javascript\" ></script>", 
           //     false);

        }

        private void LoadSettings()
        {
            litIntro.Text = config.IntroContent;
          
            //cacheDependencyKey = "Module-" + ModuleId.ToString();
            
            if (IsEditable)
            {
                LinkImage = ImageSiteRoot + "/Data/SiteImages/" + EditContentImage;
                DeleteImage = ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage;

            }

            webSnaprKey = config.WebSnaprKey;
            if (webSnaprKey.Length == 0)
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if ((siteSettings != null) && (siteSettings.WebSnaprKey.Length > 0))
                {
                    webSnaprKey = siteSettings.WebSnaprKey;
                }

            }

            if (config.AddWebSnaprCssToLinks) { linkCssClass += " websnapr"; }

            if (config.UseAjaxPaging)
            {
                pgr.Command += new CommandEventHandler(pgr_Command);  
            }
            else
            {
                pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
                rptLinks.EnableViewState = false;
                rptDescription.EnableViewState = false;
            }


        }


        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            //this.rptLinks.ItemCommand += new RepeaterCommandEventHandler(rptLinks_ItemCommand);
            //this.rptLinks.ItemDataBound += new RepeaterItemEventHandler(rptLinks_ItemDataBound);
            //this.rptDescription.ItemCommand += new RepeaterCommandEventHandler(rptLinks_ItemCommand);
            //this.rptDescription.ItemDataBound += new RepeaterItemEventHandler(rptLinks_ItemDataBound);


#if NET35
            if (WebConfigSettings.DisablePageViewStateByDefault) {Page.EnableViewState = true; }
#endif
        }



        #endregion

    }
}