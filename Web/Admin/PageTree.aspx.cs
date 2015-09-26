/// Author:					Joe Audette
/// Created:				2004-09-27
/// Last Modified:			2012-05-07
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
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.SearchIndex;
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class PageTreePage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PageTreePage));

        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private bool canEditAnything = false;
        private int selectedPage = -1;
        private ArrayList sitePages = new ArrayList();
        private SiteMapDataSource siteMapDataSource;
        private bool userCanAddPages = false;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected string EditPropertiesImage = WebConfigSettings.EditPropertiesImage;
        protected string DeleteLinkImage = WebConfigSettings.DeleteLinkImage;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            
            PopulatePageArray();
            PopulateLabels();
            PopulateControls();
        }


        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            litWarning.Text = string.Empty;

            PopulatePageList();

            if (selectedPage > -1)
            {
                ListItem listItem = lbPages.Items.FindByValue(selectedPage.ToInvariantString());
                if (listItem != null)
                {
                    lbPages.ClearSelection();
                    listItem.Selected = true;
                }
            }

            if ((!userCanAddPages)&&(!isAdmin))
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

        }

        private void PopulatePageList()
        {
            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;

            PopulateListControl(lbPages, siteMapNode, string.Empty);

        }

        private void PopulateListControl(
            ListControl listBox, 
            SiteMapNode siteMapNode, 
            string pagePrefix)
        {
            mojoSiteMapNode mojoNode = (mojoSiteMapNode)siteMapNode;

            if (!mojoNode.IsRootNode)
            {
                if ((isAdmin)
                        || ((isContentAdmin) && (mojoNode.ViewRoles != "Admins;"))
                        || ((isSiteEditor) && (mojoNode.ViewRoles != "Admins;"))
                        || (WebUser.IsInRoles(mojoNode.CreateChildPageRoles))
                        )
                {
                    if (mojoNode.ParentId > -1) pagePrefix += "-";
                    ListItem listItem = new ListItem();
                    listItem.Text = pagePrefix + Server.HtmlDecode(mojoNode.Title);
                    listItem.Value = mojoNode.PageId.ToInvariantString();

                    listBox.Items.Add(listItem);
                    userCanAddPages = true;
                }
            }


            foreach (SiteMapNode childNode in mojoNode.ChildNodes)
            {
                //recurse to populate children
                PopulateListControl(listBox, childNode, pagePrefix);

            }


        }

        

        void btnTopBottom_Click(object sender, ImageClickEventArgs e)
        {
            string cmd = ((ImageButton)sender).CommandName;

            if (lbPages.SelectedIndex > -1)
            {
                foreach (mojoSiteMapNode page in sitePages)
                {
                    if (
                        (page.PageId.ToString() == lbPages.SelectedValue)
                        && ((canEditAnything) || (WebUser.IsInRoles(page.CreateChildPageRoles)))
                        )
                    {
                        selectedPage = page.PageId;

                        PageSettings pageSettings = new PageSettings(siteSettings.SiteId, page.PageId);
                        if (cmd == "top")
                        {
                            pageSettings.MoveToTop();
                        }
                        else
                        {
                            pageSettings.MoveToBottom();
                        }
                    }
                }

                CacheHelper.ResetSiteMapCache();

                if (selectedPage > -1)
                {
                    WebUtils.SetupRedirect(this, SiteRoot + "/Admin/PageTree.aspx?selpage=" + selectedPage.ToInvariantString());
                }
                else
                {
                    WebUtils.SetupRedirect(this, Page.Request.RawUrl);
                }
            }
            else
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
        }


        private void UpDown_Click(Object sender, ImageClickEventArgs e)
        {
            string cmd = ((ImageButton) sender).CommandName;

            if (lbPages.SelectedIndex > -1)
            {
                foreach (mojoSiteMapNode page in sitePages)
                {
                    if (
                        (page.PageId.ToInvariantString() == lbPages.SelectedValue)
                        &&((canEditAnything) ||(WebUser.IsInRoles(page.CreateChildPageRoles)))
                        )
                    {
                        selectedPage = page.PageId;

                        PageSettings pageSettings = new PageSettings(siteSettings.SiteId, page.PageId);
                        if (cmd == "down")
                        {
                            pageSettings.MoveDown();
                        }
                        else
                        {
                            pageSettings.MoveUp();
                        }
                    }
                }

                CacheHelper.ResetSiteMapCache();

                if (selectedPage > -1)
                {
                    WebUtils.SetupRedirect(this, SiteRoot + "/Admin/PageTree.aspx?selpage=" + selectedPage.ToInvariantString());
                }
                else
                {
                    WebUtils.SetupRedirect(this, Page.Request.RawUrl);
                }
            }
            else
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
        }


        private void btnDelete_Click(Object sender, ImageClickEventArgs e)
        {
            if (lbPages.SelectedIndex > -1)
            {
                ContentMetaRespository metaRepository = new ContentMetaRespository();

                foreach (mojoSiteMapNode page in sitePages)
                {
                    if ((page.PageId.ToString() == lbPages.SelectedValue)&&((canEditAnything)||(WebUser.IsInRoles(page.EditRoles))))
                    {
                        if (WebConfigSettings.LogIpAddressForContentDeletions)
                        {
                            log.Info("user deleted page " + page.Url + " from ip address " + SiteUtils.GetIP4Address());

                        }

                        PageSettings pageSettings = new PageSettings(siteSettings.SiteId, page.PageId);
                        metaRepository.DeleteByContent(page.PageGuid);
                        Module.DeletePageModules(page.PageId);
                        PageSettings.DeletePage(page.PageId);
                        FriendlyUrl.DeleteByPageGuid(page.PageGuid);
                        
                        mojoPortal.SearchIndex.IndexHelper.ClearPageIndexAsync(pageSettings);
                    }
                }

                CacheHelper.ResetSiteMapCache();

                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
            else
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
        }

        void btnSortChildPagesAlpha_Click(object sender, ImageClickEventArgs e)
        {
            if (lbPages.SelectedIndex > -1)
            {
                foreach (mojoSiteMapNode page in sitePages)
                {
                    if (
                        (page.PageId.ToInvariantString() == lbPages.SelectedValue)
                        && ((canEditAnything) || (WebUser.IsInRoles(page.CreateChildPageRoles)))
                        )
                    {
                        PageSettings pageSettings = new PageSettings(siteSettings.SiteId, page.PageId);
                        pageSettings.ResortPagesAlphabetically();

                        
                    }
                }

                CacheHelper.ResetSiteMapCache();

                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
            else
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }

        }


        private void btnEdit_Click(Object sender, ImageClickEventArgs e)
        {
            if (lbPages.SelectedIndex == -1)
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
            else
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Admin/PageLayout.aspx?pageid=" + lbPages.SelectedValue);
            }
        }


        private void btnSettings_Click(object sender, ImageClickEventArgs e)
        {
            if (lbPages.SelectedIndex != -1)
            {
                foreach (mojoSiteMapNode page in sitePages)
                {
                    if (page.PageId.ToString() == lbPages.SelectedValue)
                    {
                        WebUtils.SetupRedirect
                            (this, SiteRoot + "/Admin/PageSettings.aspx?pageid=" + page.PageId.ToInvariantString());
                        break;
                    }
                }
            }
            else
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
        }

        void btnViewPage_Click(object sender, ImageClickEventArgs e)
        {
            if (lbPages.SelectedIndex != -1)
            {
                foreach (mojoSiteMapNode page in sitePages)
                {
                    if (page.PageId.ToString() == lbPages.SelectedValue)
                    {
                        WebUtils.SetupRedirect(this, page.Url);
                        break;
                    }
                }
            }
            else
            {
                // no page selected
                litWarning.Text = Resource.PagesNoSelectionWarning;
            }
        }


        private void PopulateLabels()
        {
            if (canEditAnything)
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuPageTreeLink);
            }
            else
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PageTreeTitle);
                
            }

            heading.Text = Resource.AdminMenuPageTreeLink;
            if ((!isAdmin) && (!isSiteEditor) && (!isContentAdmin))
            {
                lnkNewPage.Visible = false;
                divAdminLinks.Visible = false;
                heading.Text = Resource.PageTreeTitle;

            }

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            

            lnkPageTree.Text = Resource.AdminMenuPageTreeLink;
            lnkPageTree.ToolTip = Resource.AdminMenuPageTreeLink;
            lnkPageTree.NavigateUrl = SiteRoot + "/Admin/PageTree.aspx";

            lnkNewPage.InnerText = Resource.PagesAddButton;
            lnkNewPage.HRef = Page.ResolveUrl(SiteRoot + "/Admin/PageSettings.aspx");

            btnTop.ImageUrl = ImageSiteRoot + "/Data/SiteImages/up2.gif";
            btnTop.AlternateText = Resource.MoveToTop;
            btnTop.ToolTip = Resource.MoveToTop;

            btnUp.AlternateText = Resource.PagesUpButtonAlternateText;
            btnUp.ToolTip = Resource.PagesUpButtonAlternateText;
            btnUp.ImageUrl = ImageSiteRoot + "/Data/SiteImages/up.gif";

            btnDown.AlternateText = Resource.PagesDownButtonAlternateText;
            btnDown.ToolTip = Resource.PagesDownButtonAlternateText;
            btnDown.ImageUrl = ImageSiteRoot + "/Data/SiteImages/dn.gif";

            btnBottom.ImageUrl = ImageSiteRoot + "/Data/SiteImages/dn2.gif";
            btnBottom.AlternateText = Resource.MoveToBottom;
            btnBottom.ToolTip = Resource.MoveToBottom;

            btnEdit.AlternateText = Resource.PagesEditAlternateText;
            btnEdit.ToolTip = Resource.PagesEditAlternateText;
            btnEdit.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditContentImage;
            
            btnSettings.AlternateText = Resource.PagesEditSettingsAlternateText;
            btnSettings.ToolTip = Resource.PagesEditSettingsAlternateText;
            btnSettings.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditPropertiesImage;

            btnViewPage.AlternateText = Resource.PageViewPageLink;
            btnViewPage.ToolTip = Resource.PageViewPageLink;
            btnViewPage.ImageUrl = ImageSiteRoot + "/Data/SiteImages/search.gif";

            btnDelete.AlternateText = Resource.PagesDeleteAlternateText;
            btnDelete.ToolTip = Resource.PagesDeleteAlternateText;
            btnDelete.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage;
            UIHelper.AddConfirmationDialog(btnDelete, Resource.PageTreeDeleteWarning);

            btnSortChildPagesAlpha.ImageUrl = ImageSiteRoot + "/Data/SiteImages/sort_az.png";
            btnSortChildPagesAlpha.AlternateText = Resource.SortChildPagesAlphabetically;
            btnSortChildPagesAlpha.ToolTip = Resource.SortChildPagesAlphabetically;

            
        }

        private void PopulatePageArray()
        {
            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider
                    = "mojosite" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;
            mojoSiteMapProvider.PopulateArrayList(sitePages, siteMapNode);

        }

        private void LoadSettings()
        {
            isAdmin = WebUser.IsAdmin;
            isContentAdmin = WebUser.IsContentAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();

            canEditAnything = (isAdmin || isContentAdmin || isSiteEditor);

            selectedPage = WebUtils.ParseInt32FromQueryString("selpage", -1);

            

            AddClassToBody("administration");
            AddClassToBody("pagetree");
            

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUp.Click += new ImageClickEventHandler(this.UpDown_Click);
            this.btnDown.Click += new ImageClickEventHandler(this.UpDown_Click);
            this.btnEdit.Click += new ImageClickEventHandler(this.btnEdit_Click);
            this.btnSettings.Click += new ImageClickEventHandler(btnSettings_Click);
            this.btnDelete.Click += new ImageClickEventHandler(this.btnDelete_Click);
            btnViewPage.Click += new ImageClickEventHandler(btnViewPage_Click);

            btnTop.Click += new ImageClickEventHandler(btnTopBottom_Click);
            btnBottom.Click += new ImageClickEventHandler(btnTopBottom_Click);

            btnSortChildPagesAlpha.Click += new ImageClickEventHandler(btnSortChildPagesAlpha_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
        }

        

        

        

        #endregion
    }
}
