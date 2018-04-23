// Author:						Kevin Needham
// Created:					    2009-06-23
// Last Modified:               2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using System.Globalization;
using mojoPortal.Web.Framework;
using Resources;
using System.Configuration;

namespace mojoPortal.Web.AdminUI
{
    public partial class PendingPagesPage : NonCmsBasePage
    {
        
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 0;
        

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();
            if (!WebUser.IsAdminOrContentAdminOrContentPublisherOrContentAuthor)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }
            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            BindGrid();

        }

        private void BindGrid()
        {
            using (IDataReader reader = PageSettings.GetPendingPageListPage(
                siteSettings.SiteGuid,
                pageNumber,
                pageSize,
                out totalPages))
            {
                if (this.totalPages > 1)
                {
                    string pageUrl = SiteRoot + "/Admin/PendingPages.aspx?pagenumber={0}";

                    pgrPendingPages.Visible = true;
                    pgrPendingPages.PageURLFormat = pageUrl;
                    pgrPendingPages.ShowFirstLast = true;
                    pgrPendingPages.CurrentIndex = pageNumber;
                    pgrPendingPages.PageSize = pageSize;
                    pgrPendingPages.PageCount = totalPages;

                }
                else
                {
                    pgrPendingPages.Visible = false;
                }

                grdPendingPages.DataSource = reader;
                grdPendingPages.PageIndex = pageNumber;
                grdPendingPages.PageSize = pageSize;
                grdPendingPages.DataBind();
            }
            
        }

        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PendingPagesTitle);

            grdPendingPages.Columns[0].HeaderText = Resource.PendingPagesGridPageColumn;
            grdPendingPages.Columns[1].HeaderText = Resource.PendingPagesGridWipTotalColumn;

            grdPendingPages.Columns[2].Visible = false;

            heading.Text = Resource.PendingPagesTitle;
            ltlIntroduction.Text = Resource.PendingPagesIntroduction;

            lnkContentWorkFlow.Text = Resource.AdminMenuContentWorkflowLabel;
            lnkAdminMenu.Text = Resource.AdminMenuLink;

            lnkCurrentPage.Text = Resource.PendingPagesTitle;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/PendingPages.aspx";
        }

        protected string EditPageSettingLinkText
        {
            get
            {
                if (WebConfigSettings.UseIconsForAdminLinks)
                {
                    return String.Format("<img src='{0}' alt='{1}' />", Page.ResolveUrl("~/Data/SiteImages/"
                            + ConfigurationManager.AppSettings["EditPropertiesImage"]), Resource.PagePropertiesEditText);
                }
                else
                {
                    return Resource.PagePropertiesEditText;
                }
            }
        }

        private void LoadSettings()
        {
         
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

            AddClassToBody("administration");
            AddClassToBody("wfadmin");

        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            
                                   
            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeJQTable = true;
        }

        

        

        #endregion

    }
}
