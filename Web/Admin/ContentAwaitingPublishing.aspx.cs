// Author:					Joe Davis
// Created:				    2013-01-18
// Last Modified:			2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.AdminUI
{
    public partial class ContentAwaitingPublishingPage : NonCmsBasePage
    {
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
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
            if (!WebUser.IsAdminOrContentAdminOrContentPublisher && !WebUser.IsInRoles(WebConfigSettings.RolesAllowedToUseWorkflowAdminPages))
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
                ContentWorkflowStatus.AwaitingPublishing,
                pageNumber,
                pageSize,
                out totalPages);
            
            string pageUrl = SiteRoot + "/Admin/ContentAwaitingPublishing.aspx?pagenumber={0}";

            pgrContentAwaitingPublishing.Visible = (this.totalPages > 1);
            pgrContentAwaitingPublishing.PageURLFormat = pageUrl;
            pgrContentAwaitingPublishing.ShowFirstLast = true;
            pgrContentAwaitingPublishing.CurrentIndex = pageNumber;
            pgrContentAwaitingPublishing.PageSize = pageSize;
            pgrContentAwaitingPublishing.PageCount = totalPages;

            grdContentAwaitingPublishing.DataSource = dsWorkflows.Tables["WorkFlows"];
            grdContentAwaitingPublishing.DataBind();

        }

        protected void grdContentAwaitingPublishing_RowDataBound(object sender, GridViewRowEventArgs e)
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
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AwaitingPublishingHeading);
            
            grdContentAwaitingPublishing.Columns[0].HeaderText = Resource.ApprovalProcessGridModuleTitleLabel;
            grdContentAwaitingPublishing.Columns[1].HeaderText = Resource.RequestedBy;
            grdContentAwaitingPublishing.Columns[2].HeaderText = Resource.RequestedDate;
            grdContentAwaitingPublishing.Columns[3].HeaderText = Resource.ApprovalProcessGridModulesPublishedOnPagesLabel;

            heading.Text = Resource.AwaitingPublishingHeading;
            ltlIntroduction.Text = Resource.AwaitingPublishingIntroduction;

            lnkContentWorkFlow.Text = Resource.AdminMenuContentWorkflowLabel;
            lnkContentWorkFlow.NavigateUrl = SiteRoot + "/Admin/ContentWorkflow.aspx";
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCurrentPage.Text = Resource.AwaitingPublishingHeading;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/ContentAwaitingPublishing.aspx";
        }

        private void LoadSettings()
        {
            timeZone = SiteUtils.GetUserTimeZone();
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

            AddClassToBody("administration");
            AddClassToBody("wfadmin");
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.grdContentAwaitingPublishing.RowDataBound += new GridViewRowEventHandler(grdContentAwaitingPublishing_RowDataBound);
                       
            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeJQTable = true;
        }

        

        #endregion

    }
}
