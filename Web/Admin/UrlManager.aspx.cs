/// Author:					
/// Created:				2005-06-01
/// Last Modified:			2018-03-28
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
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class UrlManagerPage : NonCmsBasePage
    {
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 15;
        protected string RootUrl = string.Empty;
        private SiteMapDataSource siteMapDataSource;
        private Collection<DictionaryEntry> pageList;
        private bool isAdminOrContentAdmin = false;
        private bool isSiteEditor = false;
        protected string EditPropertiesImage = "~/Data/SiteImages/" + WebConfigSettings.EditPropertiesImage;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
        private string searchTerm = string.Empty;

        protected Collection<DictionaryEntry> PageList
        {
            get { return pageList; }

        }

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

            if ((!isAdminOrContentAdmin) && (!isSiteEditor))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            if (!Page.IsPostBack)
            {
                PopulateControls();
            }

        }

        private void PopulateControls()
        {
            if (pageList != null)
            {
                ddPages.DataSource = pageList;
                ddPages.DataBind();
            }

            //DataTable dataTable = FriendlyUrl.GetBySite(siteSettings.SiteId);
            //dlUrlMap.DataSource = dataTable;
            //dlUrlMap.DataBind();
            BindGrid();

            btnAddExpert.Enabled = (ddPages.Items.Count > 0);
            btnAddFriendlyUrl.Enabled = (ddPages.Items.Count > 0);

        }

        private void BindGrid()
        {
            if (searchTerm.Length > 0)
            {
                BindForSearch();
            }
            else
            {
                BindNormal();
            }

        }

        private void BindNormal()
        {
            using (IDataReader reader = FriendlyUrl.GetPage(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                out totalPages))
            {
                if (this.totalPages > 1)
                {
                    string pageUrl = SiteRoot + "/Admin/UrlManager.aspx?pagenumber={0}";

                    pgrFriendlyUrls.PageURLFormat = pageUrl;
                    pgrFriendlyUrls.ShowFirstLast = true;
                    pgrFriendlyUrls.CurrentIndex = pageNumber;
                    pgrFriendlyUrls.PageSize = pageSize;
                    pgrFriendlyUrls.PageCount = totalPages;

                }
                else
                {
                    pgrFriendlyUrls.Visible = false;
                }

                dlUrlMap.DataSource = reader;
                dlUrlMap.DataBind();
            }

        }

        private void BindForSearch()
        {
            using (IDataReader reader = FriendlyUrl.GetPage(
                siteSettings.SiteId,
                searchTerm,
                pageNumber,
                pageSize,
                out totalPages))
            {
                if (this.totalPages > 1)
                {
                    string pageUrl = SiteRoot + "/Admin/UrlManager.aspx?pagenumber={0}&amp;s=" + Server.UrlEncode(searchTerm);

                    pgrFriendlyUrls.PageURLFormat = pageUrl;
                    pgrFriendlyUrls.ShowFirstLast = true;
                    pgrFriendlyUrls.CurrentIndex = pageNumber;
                    pgrFriendlyUrls.PageSize = pageSize;
                    pgrFriendlyUrls.PageCount = totalPages;

                }
                else
                {
                    pgrFriendlyUrls.Visible = false;
                }

                dlUrlMap.DataSource = reader;
                dlUrlMap.DataBind();
            }
        }

        void btnSearchUrls_Click(object sender, EventArgs e)
        {
            string pageUrl = SiteRoot + "/Admin/UrlManager.aspx?s=" + Server.UrlEncode(txtSearch.Text);
            WebUtils.SetupRedirect(this, pageUrl);

        }

        private void btnAddFriendlyUrl_Click(object sender, EventArgs e)
        {
            if (this.txtFriendlyUrl.Text.Length > 0)
            {
                if (WebPageInfo.IsPhysicalWebPage("~/" + txtFriendlyUrl.Text))
                {
                    this.lblError.Text = Resource.FriendlyUrlWouldMaskPhysicalPageWarning;
                    return;
                }

                if (FriendlyUrl.Exists(siteSettings.SiteId, txtFriendlyUrl.Text))
                {
                    this.lblError.Text = Resource.FriendlyUrlDuplicateWarning;
                    return;
                }

                if (FriendlyUrl.Exists(siteSettings.SiteId, txtFriendlyUrl.Text.ToLower()))
                {
                    this.lblError.Text = Resource.FriendlyUrlDuplicateWarning;
                    return;
                }

                FriendlyUrl url = new FriendlyUrl();
                url.SiteId = siteSettings.SiteId;
                url.SiteGuid = siteSettings.SiteGuid;

                int pageId = -1;
                if (int.TryParse(ddPages.SelectedValue, out pageId))
                {
                    if (pageId > -1)
                    {
                        PageSettings page = new PageSettings(siteSettings.SiteId, pageId);
                        url.PageGuid = page.PageGuid;
                    }

                }

                url.Url = this.txtFriendlyUrl.Text;
                url.RealUrl = "Default.aspx?pageid=" + ddPages.SelectedValue;
                url.Save();

                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
            else
            {
                this.lblError.Text = Resource.FriendlyUrlInvalidFriendlyUrlMessage;
            }

        }

        void btnAddExpert_Click(object sender, EventArgs e)
        {
            if (
                (this.txtFriendlyUrl.Text.Length > 0)
                && (this.txtRealUrl.Text.Length > 0)
                )
            {

                if (WebPageInfo.IsPhysicalWebPage("~/" + txtFriendlyUrl.Text))
                {
                    this.lblError.Text = Resource.FriendlyUrlWouldMaskPhysicalPageWarning;
                    return;
                }

                if (FriendlyUrl.Exists(siteSettings.SiteId, txtFriendlyUrl.Text))
                {
                    this.lblError.Text = Resource.FriendlyUrlDuplicateWarning;
                    return;
                }

                FriendlyUrl url = new FriendlyUrl();
                url.SiteId = siteSettings.SiteId;
                url.SiteGuid = siteSettings.SiteGuid;
                url.Url = this.txtFriendlyUrl.Text;
                url.RealUrl = this.txtRealUrl.Text;
                url.Save();

                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
            else
            {
                this.lblError.Text = Resource.FriendlyUrlInvalidEntryMessage;
            }
        }


        private void dlUrlMap_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int urlID = Convert.ToInt32(dlUrlMap.DataKeys[e.Item.ItemIndex]);
            FriendlyUrl friendlyUrl = new FriendlyUrl(urlID);

            switch (e.CommandName)
            {
                case "edit":
                    dlUrlMap.EditItemIndex = e.Item.ItemIndex;
                    Control c = e.Item.FindControl("ddPagesEdit");
                    DropDownList dd = null;
                    if ((c != null) && (pageList != null))
                    {
                        dd = (DropDownList)c;
                        dd.DataSource = pageList;
                        dd.DataBind();

                    }
                    PopulateControls();
                    if (dd != null)
                    {
                        String selection = ParsePageId(friendlyUrl.RealUrl);
                        if (selection.Length > 0)
                        {
                            ListItem listItem = dd.Items.FindByValue(selection);
                            if (listItem != null)
                            {
                                dd.ClearSelection();
                                listItem.Selected = true;
                            }
                        }
                    }
                    break;

                case "apply":

                    TextBox txtItemFriendlyUrl 
                        = (TextBox)e.Item.FindControl("txtItemFriendlyUrl");

                    if (txtItemFriendlyUrl.Text.Length > 0)
                    {
                        Control cEdit = e.Item.FindControl("ddPagesEdit");
                        if (cEdit != null)
                        {
                            DropDownList ddEdit = (DropDownList)cEdit;
                            friendlyUrl.Url = txtItemFriendlyUrl.Text;
                            if (WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrl.Url))
                            {
                                this.lblError.Text = Resource.FriendlyUrlWouldMaskPhysicalPageWarning;
                                return;
                            }


                            int pageId = -1;
                            if (int.TryParse(ddEdit.SelectedValue, out pageId))
                            {
                                if (pageId > -1)
                                {
                                    PageSettings page = new PageSettings(siteSettings.SiteId, pageId);
                                    friendlyUrl.PageGuid = page.PageGuid;
                                }

                            }

                            friendlyUrl.RealUrl = "Default.aspx?pageid=" + ddEdit.SelectedValue;
                            friendlyUrl.Save();

                        }

                        WebUtils.SetupRedirect(this, Request.RawUrl);
                    }
                    else
                    {
                        this.lblError.Text = Resource.FriendlyUrlInvalidFriendlyUrlMessage;

                    }
                    break;

                case "applymanual":

                    if (
                        (
                            (((TextBox)e.Item.FindControl("txtItemFriendlyUrl")).Text.Length > 0)
                        )
                        && (
                            (((TextBox)e.Item.FindControl("txtItemRealUrl")).Text.Length > 0)
                            )
                      )
                    {
                        friendlyUrl.Url = ((TextBox)e.Item.FindControl("txtItemFriendlyUrl")).Text;
                        if (WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrl.Url))
                        {
                            this.lblError.Text = Resource.FriendlyUrlWouldMaskPhysicalPageWarning;
                            return;
                        }
                        friendlyUrl.RealUrl = ((TextBox)e.Item.FindControl("txtItemRealUrl")).Text;
                        friendlyUrl.PageGuid = Guid.Empty;
                        friendlyUrl.Save();
                        WebUtils.SetupRedirect(this, Request.RawUrl);
                    }
                    else
                    {
                        this.lblError.Text = Resource.FriendlyUrlInvalidEntryMessage;
                    }
                    break;

                case "delete":

                    FriendlyUrl.DeleteUrl(urlID);
                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    break;

                case "cancel":
                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    break;

            }

        }


        void dlUrlMap_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ImageButton btnDeleteUrl = e.Item.FindControl("btnDeleteUrl") as ImageButton;
            UIHelper.AddConfirmationDialog(btnDeleteUrl, Resource.FriendlyUrlDeleteConfirmWarning);
        }


        protected String ParsePageId(String stringToParse)
        {
            String result = String.Empty;

            if ((stringToParse.Length > 0) && (stringToParse.IndexOf("pageid=") > -1))
            {
                result = stringToParse.Remove(0, stringToParse.IndexOf("pageid=") + 7);

            }

            return result;

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuUrlManagerLink);

            heading.Text = Resource.AdminMenuUrlManagerLink;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkUrlManager.Text = Resource.AdminMenuUrlManagerLink;
            lnkUrlManager.NavigateUrl = SiteRoot + "/Admin/UrlManager.aspx";
            
            btnAddFriendlyUrl.Text = Resource.FriendlyUrlAddNewLabel;
            SiteUtils.SetButtonAccessKey
                (btnAddFriendlyUrl, AccessKeys.FriendlyUrlAddNewLabelAccessKey);

            btnAddExpert.Text = Resource.FriendlyUrlAddNewLabel;

            lblFriendlyUrlRoot.Text = this.RootUrl;
            subHeading.Text = Resource.FriendlyUrlAddNewLabel;

            btnSearchUrls.Text = Resource.SearchButtonText;
            

            pageList = new Collection<DictionaryEntry>();
            PopulatePageList(pageList);

        }

        private void PopulatePageList(Collection<DictionaryEntry> deCollection)
        {
            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider
                    = "mojosite" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;

            PopulatePageDictionary(deCollection, siteMapNode, string.Empty);

        }

        private void PopulatePageDictionary(
            Collection<DictionaryEntry> deCollection,
            SiteMapNode siteMapNode,
            string pagePrefix)
        {
            mojoSiteMapNode mojoNode = (mojoSiteMapNode)siteMapNode;

            if (!mojoNode.IsRootNode)
            {
                if (mojoNode.ParentId > -1) pagePrefix += "-";

                deCollection.Add(new DictionaryEntry(
                    mojoNode.Title,
                    mojoNode.PageId.ToString())
                    );
            }


            foreach (SiteMapNode childNode in mojoNode.ChildNodes)
            {
                //recurse to populate children
                PopulatePageDictionary(deCollection, childNode, pagePrefix);

            }


        }

        private void LoadSettings()
        {
            isAdminOrContentAdmin = WebUser.IsAdminOrContentAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            pageSize = WebConfigSettings.UrlManagerPageSize;

            RootUrl = SiteRoot + "/";
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            if (Request.QueryString["s"] != null)
            {
                searchTerm = Request.QueryString["s"];
            }

            if ((!IsPostBack) && (searchTerm.Length > 0)) { txtSearch.Text = SecurityHelper.SanitizeHtml(searchTerm); }

            AddClassToBody("administration urlmanager");
            
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.Load += new EventHandler(Page_Load);
            this.dlUrlMap.ItemDataBound += new DataListItemEventHandler(dlUrlMap_ItemDataBound);
            this.dlUrlMap.ItemCommand += new DataListCommandEventHandler(dlUrlMap_ItemCommand);
            this.btnAddFriendlyUrl.Click += new EventHandler(btnAddFriendlyUrl_Click);
            this.btnAddExpert.Click += new EventHandler(btnAddExpert_Click);
            btnSearchUrls.Click += new EventHandler(btnSearchUrls_Click);

            SuppressMenuSelection();
            SuppressPageMenu();

        }

        

        #endregion
    }
}
