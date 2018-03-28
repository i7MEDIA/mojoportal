/// Author:					
/// Created:				2007-08-08
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
using System.Globalization;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class ServerLog : NonCmsBasePage
    {
        private int totalPages = 1;
        private int pageNumber = 1;
        private int pageSize = 10;
        private bool sortAscending = false;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private string timeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fff";
        protected string DeleteLinkImage = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			if ((!WebUser.IsAdmin)||(!siteSettings.IsServerAdminSite))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            
            SecurityHelper.DisableBrowserCache();

            LoadSettings();

            PopulateLabels();

            if (WebConfigSettings.UseSystemLogInsteadOfFileLog)
            {
                pnlFileLog.Visible = false;
                BindDbLog();
                if (WebConfigSettings.ShowFileLogInAdditionToSystemLog)
                {
                    pnlFileLog.Visible = true;
                    ShowFileLog();
                }
            }
            else
            {
                pnlDbLog.Visible = false;
                ShowFileLog();
            }

           

        }

        private void BindDbLog()
        {
            if (IsPostBack) { return; }

            using (IDataReader reader = GetSystemLog())
            {
                string pageUrl = SiteRoot + "/Admin/ServerLog.aspx" + "?pagenumber={0}";

                pgr.PageURLFormat = pageUrl;
                pgr.ShowFirstLast = true;
                pgr.CurrentIndex = pageNumber;
                pgr.PageSize = pageSize;
                pgr.PageCount = totalPages;
                pgr.Visible = (totalPages > 1);


                rptSystemLog.DataSource = reader;
                rptSystemLog.DataBind();
            }

        }

        private IDataReader GetSystemLog()
        {
            if (sortAscending)
            {
                return SystemLog.GetPageAscending(
                    pageNumber,
                    pageSize,
                    out totalPages);
            }
            else
            {
                return SystemLog.GetPageDescending(
                    pageNumber,
                    pageSize,
                    out totalPages);
            }
        }

        

        private void ShowFileLog()
        {
            string pathToLog = Server.MapPath("~/Data/currentlog.config");

            // file won't exist if you clear the log, it deletes the file
            // but it will be re-created on next logged event.
            if (!File.Exists(pathToLog)) return;
            
            // in case there is logging happening right now might encounter
            // an error due to file locking, try 3 times

            try
            {
                txtLog.Text = File.ReadAllText(pathToLog, Encoding.UTF8);
            }
            catch
            {
                try
                {
                    txtLog.Text = File.ReadAllText(pathToLog, Encoding.UTF8);
                }
                catch
                {
                    try
                    {
                        txtLog.Text = File.ReadAllText(pathToLog, Encoding.UTF8);
                    }
                    catch 
                    {
                        txtLog.Text = Resource.CouldNotReadLogException;
                    }
                }
            }

        }

        protected void btnClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteLog();
            }
            catch
            {
                try
                {
                    DeleteLog();
                }
                catch
                {
                    try
                    {
                        DeleteLog();
                    }
                    catch { }
                }
            }

        }

        private void DeleteLog()
        {
            string pathToLog = Server.MapPath("~/Data/currentlog.config");
            if (File.Exists(pathToLog))
            {
                File.Delete(pathToLog);
                WebUtils.SetupRedirect(this, Request.RawUrl);
            }

        }

        void btnDownloadLog_Click(object sender, EventArgs e)
        {
            string downloadPath = Page.Server.MapPath("~/Data/currentlog.config");

            if (File.Exists(downloadPath))
            {
                FileInfo fileInfo = new System.IO.FileInfo(downloadPath);
                Page.Response.AppendHeader("Content-Length", fileInfo.Length.ToString(CultureInfo.InvariantCulture));
            }

            Page.Response.AddHeader("Content-Disposition", "attachment; filename=mojoportal-log-" + DateTimeHelper.GetDateTimeStringForFileName() + ".txt");
            
            
            Page.Response.ContentType = "application/txt";
            Page.Response.Buffer = false;
            Page.Response.BufferOutput = false;
            Page.Response.TransmitFile(downloadPath);
            Page.Response.End();
        }

        void btnClearDbLOg_Click(object sender, EventArgs e)
        {
            SystemLog.DeleteAll();

            WebUtils.SetupRedirect(this, SiteRoot + "/Admin/ServerLog.aspx");

        }

        void rptSystemLog_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "deleteitem")
            {
                SystemLog.Delete(int.Parse(e.CommandArgument.ToString()));
                WebUtils.SetupRedirect(this, SiteRoot + "/Admin/ServerLog.aspx");

            }

        }

        protected string FormatDate(DateTime theDate)
        {
            if (timeZone != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(theDate, timeZone).ToString(timeFormat);

            }

            return theDate.AddHours(timeOffset).ToString(timeFormat);

        }

        protected string FormatIpAddress(string ipAddress)
        {
            if(string.IsNullOrEmpty(ipAddress)) { return ipAddress; }
             
            return "<a class='cblink' title='" + ipAddress + "' href='http://whois.arin.net/rest/ip/" + ipAddress + ".txt'>" + ipAddress + "</a>";

        }

        private void PopulateLabels()
        {
            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkServerLog.Text = Resource.AdminMenuServerLogLabel;
            lnkServerLog.ToolTip = Resource.AdminMenuServerLogLabel;
            lnkServerLog.NavigateUrl = SiteRoot + "/Admin/ServerLog.aspx";

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuServerLogLabel);
            heading.Text = Resource.AdminMenuServerLogLabel;

            lnkRefresh.NavigateUrl = SiteRoot + "/Admin/ServerLog.aspx";
            lnkRefresh.Text = Resource.RefreshLogViewerLink;
            //lnkRefresh.ToolTip = Resource.RefreshLogViewerLink;

            lnkRefresh2.NavigateUrl = SiteRoot + "/Admin/ServerLog.aspx";
            lnkRefresh2.Text = Resource.RefreshLogViewerLink;

            

            btnClearLog.Text = Resource.ServerLogClearLogButton;
            btnClearLog.ToolTip = Resource.ServerLogClearLogButton;

            btnClearDbLOg.Text = Resource.ServerLogClearLogButton;
            btnClearDbLOg.ToolTip = Resource.ServerLogClearLogButton;

            

            btnDownloadLog.Text = Resource.DownloadLogButton;

            AddClassToBody("administration");
            AddClassToBody("serverlog");

        }

        private void LoadSettings()
        {
            pageSize = WebConfigSettings.SystemLogPageSize;
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            sortAscending = WebConfigSettings.SystemLogSortAscending;
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            timeFormat = WebConfigSettings.SystemLogDateTimeFormat;
            DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;


        }

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnClearLog.Click += new EventHandler(btnClearLog_Click);
            btnDownloadLog.Click += new EventHandler(btnDownloadLog_Click);
            btnClearDbLOg.Click += new EventHandler(btnClearDbLOg_Click);
            rptSystemLog.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(rptSystemLog_ItemCommand);
            SuppressMenuSelection();
            SuppressPageMenu();
        }

        

        

        


        #endregion
    }
}
