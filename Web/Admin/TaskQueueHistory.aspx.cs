/// Author:					
/// Created:				2008-01-04
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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class TaskQueueHistoryPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TaskQueueHistoryPage));

        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 20;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private bool isSiteEditor = false;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

       

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor) && (!WebUser.IsAdminOrNewsletterAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            LoadSettings();
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

            List<TaskQueue> TaskQueueList;

            if (siteSettings.IsServerAdminSite && WebUser.IsAdmin)
            {
                TaskQueueList
                    = TaskQueue.GetPage(
                        pageNumber,
                        pageSize,
                        out totalPages);
            }
            else
            {
                TaskQueueList
                    = TaskQueue.GetPageBySite(
                        siteSettings.SiteGuid,
                        pageNumber,
                        pageSize,
                        out totalPages);

            }


            if (this.totalPages > 1)
            {

                string pageUrl = SiteRoot
                    + "/Admin/TaskQueueHistory.aspx?pagenumber={0}";

                pgrTaskQueue.PageURLFormat = pageUrl;
                pgrTaskQueue.ShowFirstLast = true;
                pgrTaskQueue.CurrentIndex = pageNumber;
                pgrTaskQueue.PageSize = pageSize;
                pgrTaskQueue.PageCount = totalPages;

            }
            else
            {
                pgrTaskQueue.Visible = false;
            }

            grdTaskQueue.DataSource = TaskQueueList;
            grdTaskQueue.PageIndex = pageNumber;
            grdTaskQueue.PageSize = pageSize;
            grdTaskQueue.DataBind();

            if (TaskQueueList.Count == 0)
            {
                lblStatus.Text = Resource.TaskQueueNoTasksMessage;
            }

        }

        void grdTaskQueue_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            Guid guid = (Guid)grid.DataKeys[e.RowIndex].Value;
            TaskQueue.Delete(guid);
            WebUtils.SetupRedirect(this, Request.RawUrl);

        }

        void btnClearHistory_Click(object sender, EventArgs e)
        {
            
            TaskQueue.DeleteCompleted();
            WebUtils.SetupRedirect(this, SiteRoot+ "/Admin/TaskQueueHistory.aspx");
            
        }

        protected string GetPercentComplete(object data)
        {
            Double d = Convert.ToDouble(data) * 100;

            return d.ToString("#0", CultureInfo.InvariantCulture) + "&#37;"; //%

        }

        protected string FormatDate(object d)
        {
            if (d == null) { return string.Empty; }

            return DateTimeHelper.Format(Convert.ToDateTime(d), timeZone, "g", timeOffset);
        }


        private void PopulateLabels()
        {
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkTaskQueueMonitor.Text = Resource.TaskQueueMonitorHeading;
            lnkTaskQueueMonitor.ToolTip = Resource.TaskQueueMonitorHeading;
            lnkTaskQueueMonitor.NavigateUrl = SiteRoot + "/Admin/TaskQueueMonitor.aspx";

            lnkThisPage.Text = Resource.TaskQueueHistoryHeading;
            lnkThisPage.ToolTip = Resource.TaskQueueHistoryHeading;
            lnkThisPage.NavigateUrl = SiteRoot + "/Admin/TaskQueueHistory.aspx";

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.TaskQueueHistoryHeading);
            heading.Text = Resource.TaskQueueHistoryHeading;

            grdTaskQueue.Columns[1].HeaderText = Resource.TaskQueueGridTaksNameHeader;
            grdTaskQueue.Columns[2].HeaderText = Resource.TaskQueueGridQueuedHeader;
            grdTaskQueue.Columns[3].HeaderText = Resource.TaskQueueGridStartedHeader;
            grdTaskQueue.Columns[4].HeaderText = Resource.TaskQueueGridLastUpdateHeader;
            grdTaskQueue.Columns[5].HeaderText = Resource.TaskQueueGridCompleteHeader;
            grdTaskQueue.Columns[6].HeaderText = Resource.TaskQueueGridCompleteProgressHeader;
            
            btnClearHistory.Text = Resource.TaskQueueClearHistoryLink;
            btnClearHistory.ToolTip = Resource.TaskQueueClearHistoryLink;
            btnClearHistory.Visible = siteSettings.IsServerAdminSite;

        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            AddClassToBody("administration");
            AddClassToBody("taskqueue");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.grdTaskQueue.RowDeleting += new GridViewDeleteEventHandler(grdTaskQueue_RowDeleting);
            this.btnClearHistory.Click += new EventHandler(btnClearHistory_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;


        }

        

        #endregion
    }
}
