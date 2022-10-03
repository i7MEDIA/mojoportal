/// Author:					    
/// Created:				    2006-05-17
/// Last Modified:			    2018-03-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using mojoPortal.SearchIndex;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class ContentManagerPage : NonCmsBasePage
    {
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SuppressMenuSelection();
            SuppressPageMenu();
            this.Load += new EventHandler(this.Page_Load);
            this.grdPages.RowEditing += new GridViewEditEventHandler(grdPages_RowEditing);
            this.grdPages.RowCancelingEdit += new GridViewCancelEditEventHandler(grdPages_RowCancelingEdit);
            this.grdPages.RowUpdating += new GridViewUpdateEventHandler(grdPages_RowUpdating);
            grdPages.RowDataBound += new GridViewRowEventHandler(grdPages_RowDataBound);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            ScriptConfig.IncludeJQTable = true;
        }

        


        private static readonly ILog log = LogManager.GetLogger(typeof(ContentManagerPage));

        private int moduleID = -1;
        private int pageID = -1;
        private Module currentModule = null;
        //protected string EditImage = ConfigurationManager.AppSettings["EditContentImage"];
        private string editSettingsImage = ConfigurationManager.AppSettings["EditPropertiesImage"];
        private bool pageHasAltContent1 = false;
        private bool pageHasAltContent2 = false;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        protected bool includeAltPanes = false;
        protected string publishLinkTitle = string.Empty;
        protected bool useDialogForEditing = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();
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

            if (moduleID == -1)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Admin/ContentCatalog.aspx");
                return;
            }

            currentModule = new Module(this.moduleID);
            if (currentModule.SiteId != siteSettings.SiteId)
            {
                //trying to edit a module that is not in this site
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (currentModule.AuthorizedEditRoles == "Admins;")
            {
                if (!isAdmin)
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }
            }

            PopulateLabels();

            if (!IsPostBack)
            {
                BindControls();
                if ((Request.UrlReferrer != null)&&(hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                   
                }
            }

        }

        private void BindControls()
        {
            
            //lblModuleTitle.Text = string.Format(CultureInfo.InvariantCulture, Resource.PublishingForFormat, currentModule.ModuleTitle);
            heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.PublishingForFormat, currentModule.ModuleTitle);
            publishLinkTitle = heading.Text;

            ModuleDefinition feature = new ModuleDefinition(currentModule.ModuleDefId);
            pnlWarning.Visible = !feature.SupportsPageReuse;

            ArrayList sitePages = new ArrayList();
            PopulatePageArray(sitePages);
            List<ModuleDecoratedSiteMapNode> pageList
             = ModuleDecoratedSiteMapNode.GetDecoratedNodes(sitePages, moduleID);

            if (pageList.Count > WebConfigSettings.TooManyPagesForGridEvents)
            {
                useDialogForEditing = true;
                grdPages.EnableViewState = false;
            }

            grdPages.DataSource = pageList;
            grdPages.DataBind();

            //log.Info("EditIndex was " + grdPages.EditIndex);
            //log.Info("bound grid " + pageList.Count);

            
        }

        void grdPages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (isAdmin) { return; } // no filtering needed for admins
            if ((e.Row.DataItem != null) && (e.Row.DataItem is ModuleDecoratedSiteMapNode))
            {
                ModuleDecoratedSiteMapNode mojoNode = e.Row.DataItem as ModuleDecoratedSiteMapNode;
                // this page is locked down for only admins and current user is not admin
                if ((mojoNode.ViewRoles == "Admins;")||(mojoNode.EditRoles == "Admins;"))
                {
                    e.Row.Visible = false;
                }

            }
        }

       
        protected void grdPages_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //this event is not firing under Mono 2009-06-27
            //log.Info("grdPages_RowEditing fired");

            if (sender == null) 
            {
                //log.Error("sender was null in grdPages_RowEditing");
                return; 
            }

            if (e == null) 
            {
                //log.Error("eventargs was null in grdPages_RowEditing");
                return; 
            }

            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            BindControls();

            //log.Info("grdPages_RowEditing complated");

        }

        protected void grdPages_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            GridView grid = (GridView)sender;
            int pageID = (int)grid.DataKeys[e.RowIndex].Value;
            CheckBox chkPublish = (CheckBox)grid.Rows[e.RowIndex].Cells[2].FindControl("chkPublished");
            DropDownList ddPanes = (DropDownList)grid.Rows[e.RowIndex].Cells[2].FindControl("ddPaneNames");
            TextBox txtOrder = (TextBox)grid.Rows[e.RowIndex].Cells[3].FindControl("txtModuleOrder");
            DatePickerControl dpBegin = (DatePickerControl)grid.Rows[e.RowIndex].Cells[4].FindControl("dpBeginDate");
            DatePickerControl dpEnd = (DatePickerControl)grid.Rows[e.RowIndex].Cells[4].FindControl("dpEndDate");
            
            string paneName = ddPanes.SelectedValue;
            int moduleOrder = int.Parse(txtOrder.Text, CultureInfo.InvariantCulture);

            DateTime beginDate;
            DateTime endDate;

            Boolean beginDateInvalid = false;

            if (!DateTime.TryParse(dpBegin.Text, out beginDate))
            {
                beginDateInvalid = true;
            }

            if (dpEnd.Text.Length > 0)
            {
                if (!DateTime.TryParse(dpEnd.Text, out endDate))
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
                beginDate = beginDateInvalid ? DateTime.UtcNow.ToLocalTime(timeZone) : beginDate.ToUtc(timeZone);
                if (endDate != DateTime.MinValue) { endDate = endDate.ToUtc(timeZone); }
            }
            else
            {
                beginDate = beginDateInvalid ? DateTime.Now.AddHours(-timeOffset) : beginDate.AddHours(-timeOffset);
                if (endDate != DateTime.MinValue) endDate = endDate.AddHours(-timeOffset);
            }
            
            PageSettings currentPage = new PageSettings(siteSettings.SiteId, pageID);
            //currentModule = new Module(moduleID);

            if (chkPublish.Checked)
            {

                Module.Publish(
                    currentPage.PageGuid,
                    currentModule.ModuleGuid,
                    currentModule.ModuleId,
                    pageID, 
                    paneName, 
                    moduleOrder, 
                    beginDate, 
                    endDate);
            }
            else
            {
                

                if (WebConfigSettings.LogIpAddressForContentDeletions)
                {
                    Module m = new Module(moduleID);
                    PageSettings contentPage = new PageSettings(siteSettings.SiteId, pageID);
                    string userName = string.Empty;
                    SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                    if (currentUser != null)
                    {
                        userName = currentUser.Name;
                    }

                    log.Info($"user {userName} removed module {m.ModuleTitle} from page {contentPage.PageName} from ip address {SiteUtils.GetIP4Address()}");

                }

                Module.DeleteModuleInstance(this.moduleID, pageID);
            }
                
            // rebuild page search index
            
            currentPage.PageIndex = CurrentPage.PageIndex;
            mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(currentPage);

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }


        protected void grdPages_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }


        void btnDelete_Click(object sender, EventArgs e)
        {
            
            ArrayList pageIDs = new ArrayList();
            ArrayList sitePages = new ArrayList();
            PopulatePageArray(sitePages);
            List<ModuleDecoratedSiteMapNode> pageList
                = ModuleDecoratedSiteMapNode.GetDecoratedNodes(sitePages, moduleID);

            foreach (ModuleDecoratedSiteMapNode decoratedPage in pageList)
            {
                if (decoratedPage.IsPublished)
                {
                    pageIDs.Add(decoratedPage.PageId);
                }

            }

            foreach (int p in pageIDs)
            {
                Module.DeleteModuleInstance(this.moduleID, p);
                mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(new PageSettings(siteSettings.SiteId, p));
            }

            Module m = new Module(moduleID);
            ModuleDefinition feature = new ModuleDefinition(m.ModuleDefId);

            if (feature.DeleteProvider.Length > 0)
            {
                try
                {
                    ContentDeleteHandlerProvider contentDeleter = ContentDeleteHandlerProviderManager.Providers[feature.DeleteProvider];
                    if (contentDeleter != null)
                    {
                        contentDeleter.DeleteContent(m.ModuleId, m.ModuleGuid);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Failed to invoke content delete handler " + feature.DeleteProvider, ex);
                }
            }

            if (WebConfigSettings.LogIpAddressForContentDeletions)
            {
                string userName = string.Empty;
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null)
                {
                    userName = currentUser.Name;
                }

                log.Info("user " + userName + " deleted module " + m.ModuleTitle + " from ip address " + SiteUtils.GetIP4Address());

            }

            Module.DeleteModule(this.moduleID);
           
            if (hdnReturnUrl.Value.Length > 0)
            {
                if (hdnReturnUrl.Value.Contains("ModuleSettings.aspx"))
                {
                    // redirecting back to module settings for a deleted module cuases an error
                    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                    return;
                }

                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetNavigationSiteRoot());
        }
       
       
        protected String GetPaneName(Object o)
        {
            if (o == null)
            {
                return "contentpane";
            }

            string paneName = o.ToString().ToLower();

            if (paneName == String.Empty)
            {
                return "contentpane";
            }

            if (!pageHasAltContent1)
            {
                if (string.Equals(paneName, "altcontent1", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "contentpane";
                }
            }

            if (!pageHasAltContent2)
            {
                if (string.Equals(paneName, "altcontent2", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "contentpane";
                }
            }

            return paneName;

        }

        protected String GetPaneAlias(Object o)
        {
            String result = String.Empty;

            if (o == null)
            {
                return result;
            }

            string str = o.ToString();

            if (str.Length == 0)
            {
                return result;
            }

            result = Resource.ContentManagerCenterColumnLabel;

            if (string.Equals(str, "leftpane", StringComparison.InvariantCultureIgnoreCase))
            {
                result = Resource.ContentManagerLeftColumnLabel;
            }

            if (string.Equals(str, "rightpane", StringComparison.InvariantCultureIgnoreCase))
            {
                result = Resource.ContentManagerRightColumnLabel;
            }

            if (string.Equals(str, "altcontent1", StringComparison.InvariantCultureIgnoreCase))
            {
                result = Resource.PageLayoutAltPanel1Label;
            }

            if (string.Equals(str, "altcontent2", StringComparison.InvariantCultureIgnoreCase))
            {
                result = Resource.PageLayoutAltPanel2Label;
            }

           
            return result;

        }

        protected String GetBeginDate(Object o)
        {
            if((o == null)||(o.ToString() == String.Empty))
            {
                if (timeZone != null)
                {
                    return DateTime.UtcNow.ToLocalTime(timeZone).ToString("g");
                }

                return DateTime.UtcNow.AddHours(timeOffset).ToString("g");
            }

            if (timeZone != null)
            {
                return Convert.ToDateTime(o).ToLocalTime(timeZone).ToString("g");
            }

            return Convert.ToDateTime(o).AddHours(timeOffset).ToString("g");

        }

        protected String GetDisplayBeginDate(Object o)
        {
            if ((o == null) || (o.ToString() == String.Empty)) { return o.ToString(); }

            if (timeZone != null)
            {
                return Convert.ToDateTime(o).ToLocalTime(timeZone).ToString("g");
            }
            return Convert.ToDateTime(o).AddHours(timeOffset).ToString("g");

        }

        protected String GetEndDate(Object o)
        {
            if ((o == null) || (o.ToString() == String.Empty)) { return o.ToString(); }

            if (timeZone != null)
            {
                return Convert.ToDateTime(o).ToLocalTime(timeZone).ToString("g");
            }
            return Convert.ToDateTime(o).AddHours(timeOffset).ToString("g");
            
        }

        protected String GetModuleOrder(Object o)
        {
            if ((o == null) || (o.ToString() == String.Empty))
            {
                return "1";
            }

            return o.ToString();

        }

        protected String GetIsPublishedImageUrl(Object o)
        {
            if (WebUtils.NullToFalse(o))
            {
                return ResolveUrl("~/Data/SiteImages/plus_circle.png");
            }
            
            return ResolveUrl("~/Data/SiteImages/minus_circle.png");
           
        }

        protected String GetIsPublishedImageAltText(Object o)
        {
            if (WebUtils.NullToFalse(o))
            {
                return Resource.ContentManagerPublishedAltText;
            }

            return Resource.ContentManagerNotPublishedAltText;

        }

        

        protected String GetUpdateButtonText()
        {
            return Resource.ContentManagerUpdateButton;
        }

        protected String GetCancelButtonText()
        {
            return Resource.ContentManagerCancelButton;
        }

        private void PopulatePageArray(ArrayList sitePages)
        {
            SiteMapDataSource siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;
            mojoSiteMapProvider.PopulateArrayList(sitePages, siteMapNode);
            


        }

        

        protected void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuContentManagerLink);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";
            lnkContentManager.Text = Resource.AdminMenuContentManagerLink;
            lnkContentManager.ToolTip = Resource.AdminMenuContentManagerLink;
            lnkContentManager.NavigateUrl = SiteRoot + "/Admin/ContentCatalog.aspx";

            if (pageID > -1)
            {
                this.lnkBackToList.NavigateUrl = SiteUtils.GetNavigationSiteRoot() + "/Default.aspx?pageid=" + pageID.ToInvariantString();
                this.lnkBackToList.Visible = true;
                this.lnkBackToList.Text = Resource.ContentManagerBackToListLink;
            }
            
            lnkModuleSettings.Visible = true;
            lnkModuleSettings.Text = Resource.ModuleEditSettings;
            lnkModuleSettings.ToolTip = Resource.ModuleEditSettings;
            lnkModuleSettings.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + editSettingsImage;
            lnkModuleSettings.NavigateUrl = SiteRoot
                + "/Admin/ModuleSettings.aspx?mid=" + this.moduleID.ToInvariantString();

            lnkEdit.Text = Resource.ContentManagerViewEditContentLabel;
            lnkEdit.NavigateUrl = SiteRoot + "/Admin/ContentManagerPreview.aspx?mid=" + this.moduleID.ToInvariantString();

            if (pageID > -1)
            {
                lnkEdit.NavigateUrl += "&pageid=" + pageID.ToInvariantString();
            }

            this.grdPages.Columns[0].HeaderText = Resource.ContentManagerPageColumnHeader;
            this.grdPages.Columns[1].HeaderText = Resource.ContentManagerPublishedColumnHeader;
            this.grdPages.Columns[2].HeaderText = Resource.ContentManagerColumnColumnHeader;
            this.grdPages.Columns[3].HeaderText = Resource.ContentManagerOrderColumnHeader;
            this.grdPages.Columns[4].HeaderText = Resource.ContentManagerPublishBeginDateColumnHeader;
            this.grdPages.Columns[5].HeaderText = Resource.ContentManagerPublishEndDateColumnHeader;

           

            btnDelete.Text = Resource.DeleteButton;
            UIHelper.AddConfirmationDialog(btnDelete, Resource.ContentManagerDeleteContentWarning);
        }


        protected Collection<DictionaryEntry> PaneList()
        {
            Collection<DictionaryEntry> paneList = new Collection<DictionaryEntry>();
            paneList.Add(new DictionaryEntry(Resource.ContentManagerCenterColumnLabel, "contentpane"));
            paneList.Add(new DictionaryEntry(Resource.ContentManagerLeftColumnLabel, "leftpane"));
            paneList.Add(new DictionaryEntry(Resource.ContentManagerRightColumnLabel, "rightpane"));

            if (pageHasAltContent1)
            {
                paneList.Add(new DictionaryEntry(Resource.PageLayoutAltPanel1Label, "altcontent1"));

            }

            if (pageHasAltContent2)
            {
                paneList.Add(new DictionaryEntry(Resource.PageLayoutAltPanel2Label, "altcontent2"));

            }

            return paneList;
        }


        private void LoadSettings()
        {
            
            isAdmin = WebUser.IsAdmin;
            if (!isAdmin)
            {
                isContentAdmin = WebUser.IsContentAdmin;
                isSiteEditor = SiteUtils.UserIsSiteEditor();
            }

            pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleID = WebUtils.ParseInt32FromQueryString("mid", -1);
            pageHasAltContent1 = this.ContainsPlaceHolder("altContent1");
            pageHasAltContent2 = this.ContainsPlaceHolder("altContent2");
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            includeAltPanes = pageHasAltContent1 || pageHasAltContent2;

            ScriptConfig.IncludeColorBox = true;

            AddClassToBody("administration");
            AddClassToBody("cmadmin");
            SetupDialogScript();
        }

        private void SetupDialogScript()
        {
            string dialogScript = "$('a.publink').colorbox({width:'600px', height:'450px', iframe:true}); ";

            ScriptManager.RegisterStartupScript(this, typeof(Page),
                   "pubdialog", "\n<script type=\"text/javascript\" >"
                   + dialogScript.ToString() + "</script>", false);

        }
    }
}
