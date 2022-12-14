// Author:					
// Created:				    2012-01-15
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
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.SearchIndex;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class ContentPublishDialog : mojoDialogBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ContentPublishDialog));
        private int moduleId = -1;
        private int pageId = -1;
        private PageSettings currentPage = null;
        private Module currentModule = null;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private bool includeAltPanes = false;

        private int moduleOrder = 1;
        private string paneName = "contentpane";
        private DateTime beginDate = DateTime.UtcNow;
        private DateTime endDate = DateTime.MinValue;
        private bool isPublished = false;

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

            if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if ((currentPage == null) || (currentModule == null))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            if (!IsPostBack) { PopulateControls(); }

        }

        private void PopulateControls()
        {
            lblPageName.Text = currentPage.PageName;
            lblModuleName.Text = currentModule.ModuleTitle;

            LoadPageModule();

            if (timeZone != null)
            {
                beginDate = beginDate.ToLocalTime(timeZone);
                if (endDate != DateTime.MinValue)
                {
                    endDate = endDate.ToLocalTime(timeZone);
                }
            }
            else
            {
                beginDate = beginDate.AddHours(timeOffset);
                if (endDate != DateTime.MinValue)
                {
                    endDate = endDate.AddHours(timeOffset);
                }
            }

            dpBeginDate.Text = beginDate.ToString("g");
            if (endDate > DateTime.MinValue)
            {
                dpEndDate.Text = endDate.ToString("g");
            }

            txtModuleOrder.Text = moduleOrder.ToInvariantString();

            chkPublished.Checked = isPublished;

            ddPaneNames.DataSource = PaneList();
            ddPaneNames.DataBind();

            ListItem item = ddPaneNames.Items.FindByValue(paneName);
            if (item != null)
            {
                ddPaneNames.ClearSelection();
                item.Selected = true;
            }
        }

        private void LoadPageModule()
        {
            DataTable dataTable = Module.GetPageModulesTable(moduleId);
            foreach (DataRow row in dataTable.Rows)
            {
                int pageID = Convert.ToInt32(row["PageID"], CultureInfo.InvariantCulture);
                if (pageID == currentPage.PageId)
                {
                    isPublished = true;
                    moduleOrder = Convert.ToInt32(row["ModuleOrder"], CultureInfo.InvariantCulture);
                    paneName = row["PaneName"].ToString();
                    if (paneName.Length == 0) { paneName = "contentpane"; }

                    if (row["PublishBeginDate"] != DBNull.Value)
                    {
                        beginDate = Convert.ToDateTime(row["PublishBeginDate"]); 
                    }
                   
                    if (row["PublishEndDate"] != DBNull.Value)
                    {
                        endDate = Convert.ToDateTime(row["PublishEndDate"]);
                    }
                }

            }

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            string paneName = ddPaneNames.SelectedValue;
            DateTime beginDate = DateTime.UtcNow;
            DateTime endDate = DateTime.MinValue;

            //Boolean beginDateInvalid = false;

            if (!DateTime.TryParse(dpBeginDate.Text, out beginDate))
            {
                //beginDateInvalid = true;
            }

            if (dpEndDate.Text.Length > 0)
            {
                if (!DateTime.TryParse(dpEndDate.Text, out endDate))
                {
                    endDate = DateTime.MinValue;
                }
            }
            else
            {
                endDate = DateTime.MinValue;
            }

            if (timeZone != null)
            {
                beginDate = beginDate.ToUtc(timeZone);
                if (endDate != DateTime.MinValue) { endDate = endDate.ToUtc(timeZone); }
            }
            else
            {
                beginDate = beginDate.AddHours(-timeOffset);
                if (endDate != DateTime.MinValue) endDate = endDate.AddHours(-timeOffset);
            }

            int moduleOrder = 1;

            int.TryParse(txtModuleOrder.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out moduleOrder);

            if (chkPublished.Checked)
            {

                Module.Publish(
                    currentPage.PageGuid,
                    currentModule.ModuleGuid,
                    currentModule.ModuleId,
                    pageId,
                    paneName,
                    moduleOrder,
                    beginDate,
                    endDate);
            }
            else
            {


                if (WebConfigSettings.LogIpAddressForContentDeletions)
                {
                    Module m = new Module(moduleId);
                    PageSettings contentPage = new PageSettings(SiteInfo.SiteId, pageId);
                    string userName = string.Empty;
                    SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                    if (currentUser != null)
                    {
                        userName = currentUser.Name;
                    }

                    log.Info("user " + userName + " removed module " + m.ModuleTitle + " from page " + contentPage.PageName + " from ip address " + SiteUtils.GetIP4Address());

                }

                Module.DeleteModuleInstance(moduleId, pageId);
            }

            // rebuild page search index

            currentPage.PageIndex = CurrentPage.PageIndex;
            mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(currentPage);

            pnlUpdate.Visible = false;
            pnlFinished.Visible = true;

        }


        private void LoadSettings()
        {
            isAdmin = WebUser.IsAdmin;
            if (!isAdmin)
            {
                isContentAdmin = WebUser.IsContentAdmin;
                isSiteEditor = SiteUtils.UserIsSiteEditor();
            }

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            includeAltPanes = WebUtils.ParseBoolFromQueryString("ia", includeAltPanes);

            if (WebConfigSettings.AlwaysShowAltPanesInPublishDialog) { includeAltPanes = true; }

            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            currentPage = new PageSettings(SiteInfo.SiteId, pageId);
            if (currentPage.SiteId != SiteInfo.SiteId) { currentPage = null; }
            if (currentPage != null)
            {
                if (
                    ((currentPage.AuthorizedRoles == "Admins;") || (currentPage.EditRoles == "Admins;"))
                    && (!isAdmin)
                    )
                {
                    currentPage = null;
                }
            }

            currentModule = new Module(moduleId);
            if (currentModule.SiteId != SiteInfo.SiteId) { currentModule = null; }
            if (currentModule != null)
            {
                if (
                    ((currentModule.AuthorizedEditRoles == "Admins;") || (currentModule.ViewRoles == "Admins;"))
                    && (!isAdmin)
                 )
                {
                    currentModule = null;
                }

            }

            
        
        }

        protected Collection<DictionaryEntry> PaneList()
        {
            Collection<DictionaryEntry> paneList = new Collection<DictionaryEntry>();
            paneList.Add(new DictionaryEntry(Resource.ContentManagerCenterColumnLabel, "contentpane"));
            paneList.Add(new DictionaryEntry(Resource.ContentManagerLeftColumnLabel, "leftpane"));
            paneList.Add(new DictionaryEntry(Resource.ContentManagerRightColumnLabel, "rightpane"));

            if (includeAltPanes)
            {
                paneList.Add(new DictionaryEntry(Resource.PageLayoutAltPanel1Label, "altcontent1"));

            }

            if (includeAltPanes)
            {
                paneList.Add(new DictionaryEntry(Resource.PageLayoutAltPanel2Label, "altcontent2"));

            }

            return paneList;
        }

        private void PopulateLabels()
        {
            btnSave.Text = Resource.SaveButton;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            
        }

        
    }
}