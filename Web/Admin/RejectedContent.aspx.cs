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

namespace mojoPortal.Web.AdminUI
{
    public partial class RejectedContentPage : NonCmsBasePage
    {
        protected Double timeOffset = 0;
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 0;
        private DataSet dsWorkflows;

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
            
            dsWorkflows = ContentWorkflow.GetPageOfWorkflowsWithPageInfo(
               siteSettings.SiteGuid,
               ContentWorkflowStatus.ApprovalRejected,
               pageNumber,
               pageSize,
               out totalPages);

            string pageUrl = SiteRoot + "/Admin/RejectedContent.aspx?pagenumber={0}";

            pgrRejectedContent.Visible = (this.totalPages > 1);
            pgrRejectedContent.PageURLFormat = pageUrl;
            pgrRejectedContent.ShowFirstLast = true;
            pgrRejectedContent.CurrentIndex = pageNumber;
            pgrRejectedContent.PageSize = pageSize;
            pgrRejectedContent.PageCount = totalPages;

            grdRejectedContent.DataSource = dsWorkflows.Tables["WorkFlows"];
            grdRejectedContent.DataBind();

        }

        protected void grdRejectedContent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                string workflowGuid = ((DataRowView)e.Row.DataItem).Row.ItemArray[0].ToString();
                Repeater rptPageLinks = (Repeater)e.Row.FindControl("rptPageLinks");

                if (rptPageLinks == null) { return; }

                string whereClause = string.Format("WorkflowGuid = '{0}'", workflowGuid);
                DataView dv = new DataView(dsWorkflows.Tables["PageInfo"], whereClause, "", DataViewRowState.CurrentRows);

                rptPageLinks.DataSource = dv;
                rptPageLinks.DataBind();
            }
        }

        

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.RejectedContentHeading);

            grdRejectedContent.Columns[0].HeaderText = Resource.ApprovalProcessGridModuleTitleLabel;
            grdRejectedContent.Columns[1].HeaderText = Resource.ApprovalProcessGridRejectedByLabel;
            grdRejectedContent.Columns[2].HeaderText = Resource.ApprovalProcessGridRejectedOnLabel;
            grdRejectedContent.Columns[3].HeaderText = Resource.RejectionCommentLabel;
            grdRejectedContent.Columns[4].HeaderText = Resource.ApprovalProcessGridModulesPublishedOnPagesLabel;

            heading.Text = Resource.RejectedContentHeading;
            ltlIntroduction.Text = Resource.RejectedContentIntroduction;

            lnkContentWorkFlow.Text = Resource.AdminMenuContentWorkflowLabel;
            lnkAdminMenu.Text = Resource.AdminMenuLink;

            lnkCurrentPage.Text = Resource.RejectedContentHeading;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/RejectedContent.aspx";
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

            
            this.grdRejectedContent.RowDataBound += new GridViewRowEventHandler(grdRejectedContent_RowDataBound);
                       
            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;


        }

        

        #endregion

    }
}
