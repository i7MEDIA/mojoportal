// Author:					
// Created:					2011-02-28
// Last Modified:			2018-03-28
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
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;
using System.Globalization;



namespace mojoPortal.Web.AdminUI
{

	public partial class SiteListPage : NonCmsBasePage
    {
        protected string CurrentSiteName = string.Empty;
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 15;
        protected bool showSiteIDInSiteList = false;

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			if ((!WebUser.IsAdmin) || (!siteSettings.IsServerAdminSite))
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Admin/SiteSettings.aspx");
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            BindList();

        }

        private void BindList()
        {
            using (IDataReader reader = SiteSettings.GetPageOfOtherSites(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                out totalPages))
            {

                string pageUrl = SiteRoot + "/Admin/SiteList.aspx" + "?pagenumber={0}";

                pgr.PageURLFormat = pageUrl;
                pgr.ShowFirstLast = true;
                pgr.CurrentIndex = pageNumber;
                pgr.PageSize = pageSize;
                pgr.PageCount = totalPages;
                pgr.Visible = (totalPages > 1);


                rptSites.DataSource = reader;
                rptSites.DataBind();
            }


        }

        protected string FormatSiteId(int siteId)
        {
            return string.Format(CultureInfo.InvariantCulture, Resource.SiteIdFormat, siteId);
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SiteList);
            heading.Text = Resource.SiteList;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkSiteList.Text = Resource.SiteList;
            lnkSiteList.ToolTip = Resource.SiteList;
            lnkSiteList.NavigateUrl = SiteRoot + "/Admin/SiteList.aspx";

            lnkNewSite.Text = Resource.CreateNewSite;
            lnkNewSite.NavigateUrl = SiteRoot + "/Admin/SiteSettings.aspx?SiteID=-1";
        }

        private void LoadSettings()
        {
            CurrentSiteName = string.Format(CultureInfo.InvariantCulture, Resource.ThisSiteFormat, SecurityHelper.RemoveMarkup(siteSettings.SiteName));
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
            showSiteIDInSiteList = WebConfigSettings.ShowSiteIdInSiteList;
            pageSize = WebConfigSettings.SiteListPageSize;

            AddClassToBody("administration");

        }

        


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();
            SuppressPageMenu();


        }

        #endregion
    }
}
