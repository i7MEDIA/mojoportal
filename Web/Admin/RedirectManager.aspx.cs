/// Author:					
/// Created:				2008-11-19
/// Last Modified:			2019-01-19
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;
using System.Data;

namespace mojoPortal.Web.AdminUI
{

    public partial class RedirectManagerPage : NonCmsBasePage
    {

        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 30;
        protected string RootUrl = string.Empty;
        private bool isAdminOrContentAdmin = false;
        private bool isSiteEditor = false;
        protected string EditPropertiesImage = "~/Data/SiteImages/" + WebConfigSettings.EditContentImage;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
		private string searchTerm = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
        {
			SiteUtils.ForceSsl();

			LoadSettings();
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
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
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) { return; }

			txtSearch.Text = searchTerm;

            BindGrid();

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

		private void BindForSearch()
		{
			IList redirectList = RedirectInfo.GetPage(
				siteSettings.SiteId,
				pageNumber,
				pageSize,
				out totalPages,
				searchTerm);

			if (this.totalPages > 1)
			{
				string pageUrl = SiteRoot + "/Admin/RedirectManager.aspx?pagenumber={0}&amp;s=" + Server.UrlEncode(searchTerm);

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

			dlRedirects.DataSource = redirectList;
			dlRedirects.DataBind();
		}

		void btnSearchUrls_Click(object sender, EventArgs e)
		{
			searchTerm = Server.UrlEncode(txtSearch.Text);
			string pageUrl = SiteRoot + "/Admin/RedirectManager.aspx?s=" + searchTerm;
			WebUtils.SetupRedirect(this, pageUrl);

		}

		void btnClearSearch_Click(object sender, EventArgs e)
		{
			string pageUrl = SiteRoot + "/Admin/RedirectManager.aspx";
			WebUtils.SetupRedirect(this, pageUrl);
		}

		private void BindNormal()
		{
			IList redirectList = RedirectInfo.GetPage(
				siteSettings.SiteId,
				pageNumber,
				pageSize,
				out totalPages);

			if (totalPages > 1)
			{
				string pageUrl = SiteRoot + "/Admin/RedirectManager.aspx?pagenumber={0}";
				pgrFriendlyUrls.Visible = true;
				pgrFriendlyUrls.PageURLFormat = pageUrl;
				pgrFriendlyUrls.ShowFirstLast = true;
				pgrFriendlyUrls.CurrentIndex = pageNumber;
				pgrFriendlyUrls.PageSize = pageSize;
				pgrFriendlyUrls.PageCount = totalPages;

			}

			dlRedirects.DataSource = redirectList;
			dlRedirects.DataBind();
		}

		void dlRedirects_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
            UIHelper.AddConfirmationDialog(btnDelete, Resource.RedirectDeleteWarning);

        }

        void dlRedirects_ItemCommand(object source, DataListCommandEventArgs e)
        {
            Guid rowGuid = new Guid(dlRedirects.DataKeys[e.Item.ItemIndex].ToString());
            RedirectInfo redirect = new RedirectInfo(rowGuid);

            switch (e.CommandName)
            {
                case "edit":
                    dlRedirects.EditItemIndex = e.Item.ItemIndex;
                    BindGrid();
                 
                    break;

                case "apply":

                    TextBox txtGridOldUrl = (TextBox)e.Item.FindControl("txtGridOldUrl");
                    TextBox txtGridNewUrl = (TextBox)e.Item.FindControl("txtGridNewUrl");

                    if (
                        ((txtGridOldUrl != null) && (txtGridOldUrl.Text.Length > 0))
                        && ((txtGridNewUrl != null) && (txtGridNewUrl.Text.Length > 0))
                      )
                    {
                        redirect.OldUrl = txtGridOldUrl.Text;
                        redirect.NewUrl = txtGridNewUrl.Text;
                        redirect.Save();
                        WebUtils.SetupRedirect(this, Request.RawUrl);
                    }
                    
                    break;

                case "delete":

                    RedirectInfo.Delete(rowGuid);
                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    break;

                case "cancel":
                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    break;

            }

        }


        void btnAdd_Click(object sender, EventArgs e)
        {
            bool isValid = true;
            if (txtOldUrl.Text.Length == 0) 
            {
                isValid = false;
                lblError.Text = Resource.OldUrlRequiredMessage + " ";
            }
            if (txtNewUrl.Text.Length == 0)
            { 
                isValid = false;
                lblError.Text += Resource.NewUrlRequiredMessage;
            }

            if(!isValid){return;}

            RedirectInfo redirect = new RedirectInfo();
            redirect.SiteGuid = siteSettings.SiteGuid;
            redirect.SiteId = siteSettings.SiteId;
            redirect.OldUrl = txtOldUrl.Text;
            redirect.NewUrl = txtNewUrl.Text;
            redirect.Save();

            WebUtils.SetupRedirect(this, Request.RawUrl);
            
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RedirectManagerShortLink);

            heading.Text = Resource.RedirectManagerShortLink;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkRedirectManager.Text = Resource.RedirectManagerShortLink;
            lnkRedirectManager.NavigateUrl = SiteRoot + "/Admin/RedirectManager.aspx";

            btnAdd.Text = Resource.SaveButton;
			btnSearchUrls.Text = Resource.SearchButtonText;
			btnClearSearch.Text = Resource.ClearSearch;

			txtOldUrl.Attributes.Add("placeholder", Resource.OldUrl);
			txtNewUrl.Attributes.Add("placeholder", Resource.NewUrl);                                                                                                                                                                                                                 
		}

        private void LoadSettings()
        {
            isAdminOrContentAdmin = WebUser.IsAdminOrContentAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            pageSize = WebConfigSettings.RedirectManagerPageSize;
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
			if (Request.QueryString["s"] != null)
			{
				searchTerm = Request.QueryString["s"];
			}
			RootUrl = SiteRoot + "/";
            lblSiteRoot.Text = RootUrl;
            lblSiteRoot2.Text = RootUrl;

            AddClassToBody("administration redirectmanager");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnSearchUrls.Click += new EventHandler(btnSearchUrls_Click);
            btnClearSearch.Click += new EventHandler(btnClearSearch_Click);
			dlRedirects.ItemCommand += new DataListCommandEventHandler(dlRedirects_ItemCommand);
            dlRedirects.ItemDataBound += new DataListItemEventHandler(dlRedirects_ItemDataBound);

            SuppressMenuSelection();
            SuppressPageMenu();
        }

        
        

        #endregion
    }
}
