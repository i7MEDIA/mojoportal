/// Author:					Joe Audette
/// Created:				2008-11-19
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class RedirectManagerPage : NonCmsBasePage
    {

        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 15;
        protected string RootUrl = string.Empty;
        private bool isAdminOrContentAdmin = false;
        private bool isSiteEditor = false;
        protected string EditPropertiesImage = "~/Data/SiteImages/" + WebConfigSettings.EditPropertiesImage;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if ((!isAdminOrContentAdmin) && (!isSiteEditor))
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
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

            BindGrid();

        }

        private void BindGrid()
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
           

        }

        private void LoadSettings()
        {
            isAdminOrContentAdmin = WebUser.IsAdminOrContentAdmin;
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            pageSize = WebConfigSettings.RedirectManagerPageSize;
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

            RootUrl = SiteRoot + "/";
            lblSiteRoot.Text = RootUrl;
            lblSiteRoot2.Text = RootUrl;

            AddClassToBody("administration");
            AddClassToBody("redirectmanager");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            dlRedirects.ItemCommand += new DataListCommandEventHandler(dlRedirects_ItemCommand);
            dlRedirects.ItemDataBound += new DataListItemEventHandler(dlRedirects_ItemDataBound);

            SuppressMenuSelection();
            SuppressPageMenu();
        }

        
        

        #endregion
    }
}
