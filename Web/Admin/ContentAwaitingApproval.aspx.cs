/// Author:					
/// Created:				2007-04-29
/// Last Modified:			2018-03-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// 
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
    public partial class ContentAwaitingApprovalPage : NonCmsBasePage
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
            if (!WebUser.IsAdminOrContentAdminOrContentPublisher 
                && !WebUser.IsInRoles(WebConfigSettings.RolesAllowedToUseWorkflowAdminPages)
                )
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
                ContentWorkflowStatus.AwaitingApproval,
                pageNumber,
                pageSize,
                out totalPages);
            
            string pageUrl = SiteRoot + "/Admin/ContentAwaitingApproval.aspx?pagenumber={0}";

            pgrContentAwaitingApproval.Visible = (this.totalPages > 1);
            pgrContentAwaitingApproval.PageURLFormat = pageUrl;
            pgrContentAwaitingApproval.ShowFirstLast = true;
            pgrContentAwaitingApproval.CurrentIndex = pageNumber;
            pgrContentAwaitingApproval.PageSize = pageSize;
            pgrContentAwaitingApproval.PageCount = totalPages;

            grdContentAwaitingApproval.DataSource = dsWorkflows.Tables["WorkFlows"];
            grdContentAwaitingApproval.DataBind();

        }

        protected void grdContentAwaitingApproval_RowDataBound(object sender, GridViewRowEventArgs e)
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
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AwaitingApprovalHeading);
            
            grdContentAwaitingApproval.Columns[0].HeaderText = Resource.ApprovalProcessGridModuleTitleLabel;
            grdContentAwaitingApproval.Columns[1].HeaderText = Resource.ApprovalProcessGridApprovalRequestByLabel;
            grdContentAwaitingApproval.Columns[2].HeaderText = Resource.ApprovalProcessGridApprovalRequestOnLabel;
            grdContentAwaitingApproval.Columns[3].HeaderText = Resource.ApprovalProcessGridModulesPublishedOnPagesLabel;

            heading.Text = Resource.AwaitingApprovalHeading;
            ltlIntroduction.Text = Resource.AwaitingApprovalIntroduction;

            lnkContentWorkFlow.Text = Resource.AdminMenuContentWorkflowLabel;
            lnkAdminMenu.Text = Resource.AdminMenuLink;

            lnkCurrentPage.Text = Resource.AwaitingApprovalHeading;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/ContentAwaitingApproval.aspx";
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
            this.grdContentAwaitingApproval.RowDataBound += new GridViewRowEventHandler(grdContentAwaitingApproval_RowDataBound);
                       
            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;

        }

        

        #endregion

    }
}
