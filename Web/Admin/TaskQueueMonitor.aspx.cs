/// Author:					
/// Created:				2007-12-30
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
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using mojoPortal.Web.Framework;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class TaskQueueMonitorPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TaskQueueMonitorPage));

        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 20;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private bool isSiteEditor = false;

        

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
                    = TaskQueue.GetPageUnfinished(
                        pageNumber,
                        pageSize,
                        out totalPages);
            }
            else
            {
                TaskQueueList
                    = TaskQueue.GetPageUnfinishedBySite(
                        siteSettings.SiteGuid,
                        pageNumber,
                        pageSize,
                        out totalPages);

            }


            if (this.totalPages > 1)
            {
              
                string pageUrl = SiteRoot 
                    + "/Admin/TaskQueueMonitor.aspx?pagenumber={0}";

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
                lblStatus.Text = Resource.TaskQueueNoTasksRunningMessage;
            }

        }

        void btnTest_Click(object sender, EventArgs e)
        {
            ThreadSleepTask testTask = new ThreadSleepTask();
            testTask.SiteGuid = siteSettings.SiteGuid;
            testTask.TaskName = "Test task that just sleeps a bit";
            testTask.QueueTask();

            WebUtils.SetupRedirect(this, Request.RawUrl);


        }

        void btnStartTasks_Click(object sender, EventArgs e)
        {
            WebTaskManager.StartOrResumeTasks();

            WebUtils.SetupRedirect(this, Request.RawUrl);

           
        }

        protected string GetPercentComplete(object data)
        {
            Double d = Convert.ToDouble(data) * 100;

            return d.ToString("#0", CultureInfo.InvariantCulture) + "&#37;"; //%

        }


        private void PopulateLabels()
        {
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkThisPage.Text = Resource.TaskQueueMonitorHeading;
            lnkThisPage.ToolTip = Resource.TaskQueueMonitorHeading;
            lnkThisPage.NavigateUrl = SiteRoot + "/Admin/TaskQueueMonitor.aspx";

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.TaskQueueMonitorHeading);
            heading.Text = Resource.TaskQueueMonitorHeading;

            lnkRefresh.Text = Resource.TaskQueueRefreshLink;
            lnkRefresh.ToolTip = Resource.TaskQueueRefreshLink;
            lnkRefresh.NavigateUrl = Request.RawUrl;

            lnkTaskQueueHistory.Text = Resource.TaskQueueHistoryHeading;
            lnkTaskQueueHistory.ToolTip = Resource.TaskQueueHistoryHeading;
            lnkTaskQueueHistory.NavigateUrl = SiteRoot + "/Admin/TaskQueueHistory.aspx";

            btnTest.Visible = WebConfigSettings.EnableTaskQueueTestLinks;
            btnTest.Text = "Create ThreadSleepTask";

            btnStartTasks.Visible = WebConfigSettings.EnableTaskQueueTestLinks;

            btnStartTasks.Text = "Start Tasks";

            grdTaskQueue.Columns[0].HeaderText = Resource.TaskQueueGridTaksNameHeader;
            grdTaskQueue.Columns[1].HeaderText = Resource.TaskQueueGridQueuedHeader;
            grdTaskQueue.Columns[2].HeaderText = Resource.TaskQueueGridStartedHeader;
            grdTaskQueue.Columns[3].HeaderText = Resource.TaskQueueGridLastUpdateHeader;
            grdTaskQueue.Columns[4].HeaderText = Resource.TaskQueueGridCompleteProgressHeader;
            grdTaskQueue.Columns[5].HeaderText = Resource.TaskQueueGridStatusHeader;
            
            /*
             When GetAvailableThreads returns, the variable specified by workerThreads contains the number 
             * of additional worker threads that can be started, and the variable specified by completionPortThreads 
             * contains the number of additional asynchronous I/O threads that can be started.

                If there are no available threads, additional thread pool requests remain queued until thread pool 
             * threads become available.
             */
            int workerThreads = 0;
            int completionPortThreads = 0;
            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);

            litAvailableThreads.Text = workerThreads.ToString(CultureInfo.InvariantCulture);

            

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
            this.btnTest.Click += new EventHandler(btnTest_Click);
            this.btnStartTasks.Click += new EventHandler(btnStartTasks_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;
        }

        

        

        #endregion
    }
}
