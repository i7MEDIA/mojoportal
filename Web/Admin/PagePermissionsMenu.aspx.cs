// Author:					
// Created:					2012-01-14
// Last Modified:			2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web.UI;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;



namespace mojoPortal.Web.AdminUI
{

    public partial class PagePermissionsMenuPage : NonCmsBasePage
    {
        private int pageId = -1;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
       

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

            if (!isAdmin && !isContentAdmin && !isSiteEditor)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if ((!isAdmin) && (CurrentPage.EditRoles == "Admins;"))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {
            heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.ManagePermissionsFormat, CurrentPage.PageName);
            Title = heading.Text;

            lnkPageViewRoles.Text = Resource.PageLayoutViewRolesLabel;
            lnkPageViewRoles.NavigateUrl = SiteRoot + "/Admin/PagePermission.aspx?pageid=" + pageId.ToInvariantString() + "&p=v";

            lnkPageEditRoles.Text = Resource.PageLayoutEditRolesLabel;
            lnkPageEditRoles.NavigateUrl = SiteRoot + "/Admin/PagePermission.aspx?pageid=" + pageId.ToInvariantString() + "&p=e";

            lnkPageDraftRoles.Text = Resource.PageLayoutDraftEditRolesLabel;
            lnkPageDraftRoles.NavigateUrl = SiteRoot + "/Admin/PagePermission.aspx?pageid=" + pageId.ToInvariantString() + "&p=d";

            lnkChildPageRoles.Text = Resource.PageLayoutCreateChildPageRolesLabel;
            lnkChildPageRoles.NavigateUrl = SiteRoot + "/Admin/PagePermission.aspx?pageid=" + pageId.ToInvariantString() + "&p=ce";

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null) 
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot + "/Admin/PageSettings.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + Resource.PageSettingsPageTitle
                    + "</a>" + crumbs.ItemWrapperBottom;
            }
        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            isAdmin = WebUser.IsAdmin;
            if (!isAdmin)
            {
                isContentAdmin = WebUser.IsContentAdmin;
                isSiteEditor = SiteUtils.UserIsSiteEditor();
            }

        }

       

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);

            AllowSkinOverride = (pageId > -1);
            base.OnPreInit(e);

            if (
                (pageId > -1)
                   && (siteSettings.AllowPageSkins)
                    && (CurrentPage != null)
                    && (CurrentPage.Skin.Length > 0)
                    )
            {

                if (Global.RegisteredVirtualThemes)
                {
                    this.Theme = "pageskin-" + siteSettings.SiteId.ToInvariantString() + CurrentPage.Skin;
                }
                else
                {
                    this.Theme = "pageskin";
                }
            }

        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);


            SuppressPageMenu();

        }

        #endregion
    }
}
