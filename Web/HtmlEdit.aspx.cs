/// Author:                     
/// Created:                    2004-08-29
///	Last Modified:              2013-04-24

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.SearchIndex;
using Resources;

namespace mojoPortal.Web.ContentUI
{

    public partial class EditHtml : NonCmsBasePage
    {

        private Hashtable moduleSettings;
        private HtmlContent html;
        protected int pageId = -1;
        protected int moduleId = -1;
        protected Module module = null;
        private int itemId = -1;

        private string virtualRoot;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        protected HtmlConfiguration config = new HtmlConfiguration();

        private bool enableContentVersioning = false;
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalPages = 1;
        private SiteUser currentUser = null;
        private Guid restoreGuid = Guid.Empty;
        protected bool isAdmin = false;
        private ContentWorkflow workInProgress;
        private bool userCanEdit = false;
        private bool userCanEditAsDraft = false;
        private HtmlRepository repository = null;



        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);

            if (HttpContext.Current == null) { return; }
            DoPreInit();

            //edContent.ID = "edContent";


        }

        private void DoPreInit()
        {
            //

            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }
            SecurityHelper.DisableBrowserCache();

            LoadParams();

            LoadModuleSettings();

            if (config.UseWysiwygEditor)
            {
                SiteUtils.SetupEditor(edContent, AllowSkinOverride, this);
            }
            else
            {
                SiteUtils.SetupEditor(edContent, WebConfigSettings.UseSkinCssInEditor, "TextAreaProvider", AllowSkinOverride, false, this);
            }
        }

        override protected void OnInit(EventArgs e)
        {
            if (HttpContext.Current == null) { return; }
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            DoInit();
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            btnUpdateDraft.Click += new EventHandler(btnUpdateDraft_Click);

            grdHistory.RowCommand += new GridViewCommandEventHandler(grdHistory_RowCommand);
            grdHistory.RowDataBound += new GridViewRowEventHandler(grdHistory_RowDataBound);
            pgrHistory.Command += new CommandEventHandler(pgrHistory_Command);
            btnRestoreFromGreyBox.Click += new System.Web.UI.ImageClickEventHandler(btnRestoreFromGreyBox_Click);
            btnDeleteHistory.Click += new EventHandler(btnDeleteHistory_Click);
            btnPublishDraft.Click += new EventHandler(btnPublishDraft_Click);
            ScriptConfig.IncludeJQTable = true;
        }



        private void DoInit()
        {
            SuppressPageMenu();
        }




        #endregion



        private void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current == null) { return; }

            LoadSettings();


            if ((!userCanEdit) && (!userCanEditAsDraft))
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

            if (!Page.IsPostBack)
            {
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();
                    if (hdnReturnUrl.Value.Contains("HtmlEdit.apx"))
                    {
                        lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
                    }

                }
                PopulateControls();
            }
        }

        private bool UserCanEdit(int moduleId)
        {
            bool result = UserCanEditModule(moduleId, HtmlContent.FeatureGuid);

            if ((!result) && (pageId == -1) && (module != null))
            {
                // allow it to be edited if user has edit permissions on the module
                // to support re-use with modulewrapper and still allow editing
                result = WebUser.IsInRoles(module.AuthorizedEditRoles);
            }

            return result;
        }

        private void PopulateControls()
        {
            if (html == null) { return; }
            this.itemId = html.ItemId;

            edContent.Text = html.Body;
            chkExcludeFromRecentContent.Checked = html.ExcludeFromRecentContent;

            if ((workInProgress != null) && (ViewMode == PageViewMode.WorkInProgress))
            {
                pnlWorkflowStatus.Visible = true;
                edContent.Text = workInProgress.ContentText;



                litRecentActionBy.Text = workInProgress.RecentActionByUserLogin;
                litRecentActionOn.Text = workInProgress.RecentActionOn.ToString();

                lnkCompareDraft.Visible = true;
                lnkCompareDraft.NavigateUrl = SiteRoot
                    + "/HtmlCompare.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture)
                    + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture) + "&d=" + workInProgress.Guid.ToString();

                switch (workInProgress.Status)
                {
                    case ContentWorkflowStatus.ApprovalRejected:

                        litWorkflowStatus.Text = Resource.ContentWasRejected;
                        lblRecentActionBy.ConfigKey = "RejectedBy";
                        lblRecentActionOn.ConfigKey = "RejectedOn";
                        litCreatedBy.Text = workInProgress.CreatedByUserName;
                        ltlRejectionReason.Text = workInProgress.Notes;
                        divRejection.Visible = true;

                        break;

                    case ContentWorkflowStatus.AwaitingApproval:

                        litWorkflowStatus.Text = Resource.ContentAwaitingApproval;
                        lblRecentActionBy.ConfigKey = "ContentLastEditBy";
                        lblRecentActionOn.ConfigKey = "ContentLastEditDate";
                        ltlRejectionReason.Text = string.Empty;
                        divRejection.Visible = false;

                        break;
           
                    case ContentWorkflowStatus.AwaitingPublishing:

                        litWorkflowStatus.Text = Resource.ContentAwaitingPublishing;
                        lblRecentActionBy.ConfigKey = "ApprovedBy";
                        lblRecentActionOn.ConfigKey = "ApprovedDate";

                        break;
                    case ContentWorkflowStatus.Draft:

                        litWorkflowStatus.Text = Resource.ContentEditsInProgress;
                        lblRecentActionBy.ConfigKey = "ContentLastEditBy";
                        lblRecentActionOn.ConfigKey = "ContentLastEditDate";
                        ltlRejectionReason.Text = string.Empty;
                        divRejection.Visible = false;

                        break;

                }

            }


            //if(this.itemId == -1)
            //{
            this.btnDelete.Visible = false;

            //}

            if (enableContentVersioning)
            {
                BindHistory();

            }

            if (restoreGuid != Guid.Empty)
            {
                ContentHistory rHistory = new ContentHistory(restoreGuid);
                if (rHistory.ContentGuid == html.ModuleGuid)
                {
                    edContent.Text = rHistory.ContentText;
                }

            }
        }



        private void SaveHtml(bool draft)
        {
            if (html == null) { return; }

            this.itemId = html.ItemId;

            html.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);

            bool saveHtml = true;
            string htmlBody = edContent.Text;

            if (draft)
            {
                //ContentWorkflow wip = ContentWorkflow.GetWorkInProgress(this.module.ModuleGuid);
                //dont update the actual data, but edit/create the draft version:
                if (workInProgress != null)
                {

                    if (workInProgress.Status != ContentWorkflowStatus.Draft)
                    {
                        //JOE DAVIS
                        if ((workInProgress.Status == ContentWorkflowStatus.AwaitingApproval || workInProgress.Status == ContentWorkflowStatus.AwaitingPublishing)
                            && ((userCanEdit) || UserCanApproveDraftModule(module.ModuleId, HtmlContent.FeatureGuid)))
                        {
                            // do nothing, let the editor update the draft without changing the status
                        }
                        else
                        {
                            //otherwise set the status back to draft
                            workInProgress.Status = ContentWorkflowStatus.Draft;
                        }
                    }

                    workInProgress.ContentText = edContent.Text;
                    workInProgress.LastModUserGuid = currentUser.UserGuid;
                    workInProgress.Save();
                }
                else
                {
                    //draft version doesn't exist - create it:
                    ContentWorkflow.CreateDraftVersion(
                        siteSettings.SiteGuid,
                        edContent.Text,
                        string.Empty,
                        -1,
                        Guid.Empty,
                        this.module.ModuleGuid,
                        currentUser.UserGuid);
                }

                //if this is a new item, then we want to save it even though we will be saving it with no html, 
                //this ensures that there is a record there from the start:
                if (html.ItemId < 1)
                {
                    saveHtml = true;
                    //save with no content as there will be no live content
                    htmlBody = String.Empty;
                }
                else
                {
                    saveHtml = false;
                }
            }

            if (saveHtml)
            {

                html.Body = htmlBody;
                //these will really only be saved if it is a new html instance
                html.CreatedDate = DateTime.UtcNow;
                html.UserGuid = currentUser.UserGuid;

                html.LastModUserGuid = currentUser.UserGuid; //this is only used for update

                if (divExcludeFromRecentContent.Visible)
                {
                    html.ExcludeFromRecentContent = chkExcludeFromRecentContent.Checked;
                }

                if (module != null)
                {
                    html.ModuleGuid = module.ModuleGuid;
                }
                html.LastModUtc = DateTime.UtcNow;


                if (enableContentVersioning && !draft)
                {
                    html.CreateHistory(siteSettings.SiteGuid);
                }

                repository.Save(html);
            }
            else
            {
                if ((divExcludeFromRecentContent.Visible) && (chkExcludeFromRecentContent.Checked != html.ExcludeFromRecentContent))
                {
                    html.ExcludeFromRecentContent = chkExcludeFromRecentContent.Checked;
                    repository.Save(html);
                }

            }

            if (!draft)
            {
                CurrentPage.UpdateLastModifiedTime();
                CacheHelper.ClearModuleCache(moduleId);

                SiteUtils.QueueIndexing();
            }


            //if (hdnReturnUrl.Value.Length > 0)
            //{

            //    WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
            //    return;
            //}

            //WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }


        private void btnUpdate_Click(Object sender, EventArgs e)
        {
            SaveHtml(false);

            if (hdnReturnUrl.Value.Length > 0)
            {

                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }

        void btnPublishDraft_Click(object sender, EventArgs e)
        {
            SaveHtml(true); // save the draft first

            //SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) { return; }

            HtmlContent html = repository.Fetch(moduleId);
            if (html != null)
            {
                html.ContentChanged += new ContentChangedEventHandler(html_ContentChanged);
                html.PublishDraft(siteSettings.SiteGuid, currentUser.UserGuid);
            }

            if (hdnReturnUrl.Value.Length > 0)
            {

                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }

        private void btnUpdateDraft_Click(object sender, EventArgs e)
        {
            SaveHtml(true);

            if (hdnReturnUrl.Value.Length > 0)
            {

                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }

        void html_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["HtmlContentIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }



        private void BindHistory()
        {
            if ((html == null))
            {
                pnlHistory.Visible = false;
                return;
            }

            if ((module != null) && (html.ModuleGuid == Guid.Empty))
            {
                html.ModuleGuid = module.ModuleGuid;
            }


            List<ContentHistory> history = ContentHistory.GetPage(html.ModuleGuid, pageNumber, pageSize, out totalPages);
            //using (IDataReader reader = ContentHistory.GetPageAsReader(html.ModuleGuid, pageNumber, pageSize, out totalPages))
            //{
            pgrHistory.ShowFirstLast = true;
            pgrHistory.PageSize = pageSize;
            pgrHistory.PageCount = totalPages;
            pgrHistory.Visible = (this.totalPages > 1);

            grdHistory.DataSource = history;
            //grdHistory.DataSource = reader;
            grdHistory.DataBind();
            //}

            btnDeleteHistory.Visible = (grdHistory.Rows.Count > 0);
            pnlHistory.Visible = (grdHistory.Rows.Count > 0);

        }

        void pgrHistory_Command(object sender, CommandEventArgs e)
        {
            pageNumber = Convert.ToInt32(e.CommandArgument);
            pgrHistory.CurrentIndex = pageNumber;
            BindHistory();
        }

        void grdHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string g = e.CommandArgument.ToString();
            if (g.Length != 36) { return; }
            Guid historyGuid = new Guid(g);

            switch (e.CommandName)
            {
                case "RestoreToEditor":
                    ContentHistory history = new ContentHistory(historyGuid);
                    if (history.Guid == Guid.Empty) { return; }

                    edContent.Text = history.ContentText;
                    BindHistory();
                    break;

                case "DeleteHistory":
                    ContentHistory.Delete(historyGuid);
                    BindHistory();
                    break;

                default:

                    break;
            }
        }

        void grdHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            Button btnDelete = (Button)e.Row.FindControl("btnDelete");

            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("OnClick", "return confirm('"
                    + Resource.DeleteHistoryItemWarning + "');");
            }

        }

        void btnRestoreFromGreyBox_Click(object sender, ImageClickEventArgs e)
        {
            if (hdnHxToRestore.Value.Length != 36)
            {
                BindHistory();
                return;
            }

            Guid h = new Guid(hdnHxToRestore.Value);

            ContentHistory history = new ContentHistory(h);
            if (history.Guid == Guid.Empty) { return; }

            edContent.Text = history.ContentText;
            BindHistory();

        }

        void btnDeleteHistory_Click(object sender, EventArgs e)
        {
            if (html == null) { return; }

            ContentHistory.DeleteByContent(html.ModuleGuid);
            BindHistory();
            updHx.Update();

        }

        private void SetupScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");

            script.Append("$(document).ready(function () {"); // prevent dropping a file from navigating 

            script.Append("$(document.body).bind('dragover', function(e) {");
            script.Append("e.preventDefault();");
            script.Append("return false; ");
            script.Append("});");

            script.Append("$(document.body).bind('drop', function(e) {");
            script.Append("e.preventDefault();");
            script.Append("return false; ");
            script.Append("});");

            script.Append("}); ");


            if (enableContentVersioning)
            {
                script.Append("function LoadHistoryInEditor(hxGuid) {");

                script.Append("var hdn = document.getElementById('" + this.hdnHxToRestore.ClientID + "'); ");
                script.Append("hdn.value = hxGuid; ");
                script.Append("var btn = document.getElementById('" + this.btnRestoreFromGreyBox.ClientID + "');  ");
                script.Append("btn.click(); ");
                script.Append("$.colorbox.close(); ");

                script.Append("}");
            
            }

            

            script.Append("</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page), "gbHandler", script.ToString());

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, string.Format(Resource.EditHtmlTitleFormat, GetModuleTitle(moduleId)));

            btnUpdate.Text = Resource.EditHtmlUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, AccessKeys.EditHtmlUpdateButtonAccessKey);
            UIHelper.AddClearPageExitCode(btnUpdate);
            UIHelper.AddClearPageExitCode(btnUpdateDraft);
            UIHelper.AddClearPageExitCode(btnPublishDraft);
            ScriptConfig.EnableExitPromptForUnsavedContent = true;

            lnkCancel.Text = Resource.EditHtmlCancelButton;

            btnDelete.Text = Resource.EditHtmlDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.EditHtmlDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete, Resource.HtmlDeleteContentWarning);
            edContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            litVersionHistory.Text = Resource.ContentVersionHistory;

            grdHistory.Columns[0].HeaderText = Resource.CreatedDateGridHeader;
            grdHistory.Columns[1].HeaderText = Resource.ArchiveDateGridHeader;

            btnRestoreFromGreyBox.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
            btnRestoreFromGreyBox.Attributes.Add("tabIndex", "-1");
            btnRestoreFromGreyBox.AlternateText = " ";

            btnDeleteHistory.Text = Resource.DeleteAllHistoryButton;
            UIHelper.AddConfirmationDialogWithClearExitCode(btnDeleteHistory, Resource.DeleteAllHistoryWarning);

            lnkCompareDraft.Text = Resource.CompareDraftToLiveLink;
            lnkCompareDraft.ToolTip = Resource.CompareDraftToLiveTooltip;

            btnPublishDraft.Text = Resource.ContentManagerPublishContentLink;



        }

        private void LoadParams()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            restoreGuid = WebUtils.ParseGuidFromQueryString("r", restoreGuid);



        }

        private void LoadModuleSettings()
        {
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            config = new HtmlConfiguration(moduleSettings);

        }

        private void LoadSettings()
        {
            ScriptConfig.IncludeColorBox = true;
            repository = new HtmlRepository();

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            virtualRoot = WebUtils.GetApplicationRoot();

            currentUser = SiteUtils.GetCurrentSiteUser();
            timeOffset = SiteUtils.GetUserTimeOffset();
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();



            module = GetHtmlModule();

            if (module == null)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;

            }

            if (module.ModuleTitle.Length == 0)
            {
                //this is not persisted just used for display if there is no title
                module.ModuleTitle = Resource.EditHtmlSettingsLabel;
            }


            heading.Text = Server.HtmlEncode(module.ModuleTitle);


            userCanEdit = UserCanEdit(moduleId);
            userCanEditAsDraft = UserCanOnlyEditModuleAsDraft(moduleId, HtmlContent.FeatureGuid);

            divExcludeFromRecentContent.Visible = userCanEdit;

            pageSize = config.VersionPageSize;
            enableContentVersioning = config.EnableContentVersioning;

            if ((siteSettings.ForceContentVersioning) || (WebConfigSettings.EnforceContentVersioningGlobally))
            {
                enableContentVersioning = true;
            }

            if ((WebUser.IsAdminOrContentAdmin) || (SiteUtils.UserIsSiteEditor())) { isAdmin = true; }


            edContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;


            if (moduleSettings.Contains("HtmlEditorHeightSetting"))
            {
                edContent.WebEditor.Height = Unit.Parse(moduleSettings["HtmlEditorHeightSetting"].ToString());
            }



            divHistoryDelete.Visible = (enableContentVersioning && isAdmin);

            pnlHistory.Visible = enableContentVersioning;

            
            
             SetupScript();
           

            html = repository.Fetch(moduleId);
            if (html == null)
            {
                html = new HtmlContent();
                html.ModuleId = moduleId;
                html.ModuleGuid = module.ModuleGuid;
            }

            if ((!userCanEdit) && (userCanEditAsDraft))
            {
                btnUpdate.Visible = false;
                btnUpdateDraft.Visible = true;
            }

            btnUpdateDraft.Text = Resource.EditHtmlUpdateDraftButton;

            if ((WebConfigSettings.EnableContentWorkflow) && (siteSettings.EnableContentWorkflow))
            {
                workInProgress = ContentWorkflow.GetWorkInProgress(this.module.ModuleGuid);
                //bool draftOnlyAccess = UserCanOnlyEditModuleAsDraft(moduleId);

                if (workInProgress != null)
                {
                    // let editors toggle between draft and live view in the editor
                    if (userCanEdit) { SetupWorkflowControls(true); }

                    switch (workInProgress.Status)
                    {
                        case ContentWorkflowStatus.Draft:

                            //there is a draft version currently available, therefore dont allow the non draft version to be edited:
                            btnUpdateDraft.Visible = true;
                            btnUpdate.Visible = false;
                            if (ViewMode == PageViewMode.WorkInProgress)
                            {
                                //litModuleTitle.Text += " - " + Resource.ApprovalProcessStatusDraft;
                                heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.DraftFormat, module.ModuleTitle);
                                lblContentStatusLabel.SetOrAppendCss("wf-draft"); //JOE DAVIS
                                if (userCanEdit) { btnPublishDraft.Visible = true; }
                            }



                            break;

                        case ContentWorkflowStatus.ApprovalRejected:
                            //rejected content - allow update as draft only
                            btnUpdateDraft.Visible = true;
                            btnUpdate.Visible = false;
                            if (ViewMode == PageViewMode.WorkInProgress)
                            {
                                //litModuleTitle.Text += " - " + Resource.ApprovalProcessStatusRejected;
                                heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.ContentRejectedFormat, module.ModuleTitle);
                                lblContentStatusLabel.SetOrAppendCss("wf-rejected"); //JOE DAVIS
                            }
                            break;

                        case ContentWorkflowStatus.AwaitingApproval:
                            //pending approval - dont allow any edited:
                            // 2010-01-18 let editors update the draft if they want to before approving it.
                            btnUpdateDraft.Visible = userCanEdit;

                            btnUpdate.Visible = false;
                            if (ViewMode == PageViewMode.WorkInProgress)
                            {
                                //litModuleTitle.Text += " - " + Resource.ApprovalProcessStatusAwaitingApproval;
                                heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.ContentAwaitingApprovalFormat, module.ModuleTitle);
                                lblContentStatusLabel.SetOrAppendCss("wf-awaitingapproval"); //JOE DAVIS
                            }
                            break;
                        //JOE DAVIS
                        case ContentWorkflowStatus.AwaitingPublishing:
                            //pending publishing - allow editors, publishers, admin to update before publishing
                            btnUpdateDraft.Visible = userCanEdit;

                            btnUpdate.Visible = false;

                            if (ViewMode == PageViewMode.WorkInProgress)
                            {
                                heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.ContentAwaitingPublishingFormat, module.ModuleTitle);
                                lblContentStatusLabel.SetOrAppendCss("wf-awaitingpublishing");
                            }
                            break;
                    }
                }
                else
                {
                    //workInProgress is null there is no draft
                    if (userCanEdit)
                    {
                        btnUpdateDraft.Text = Resource.CreateDraftButton;
                        btnUpdateDraft.Visible = true;
                    }

                }

                if ((userCanEdit) && (ViewMode == PageViewMode.Live))
                {
                    btnUpdateDraft.Visible = false;
                    btnUpdate.Visible = true;
                }

            }

            AddClassToBody("htmledit");

        }


        private Module GetHtmlModule()
        {
            Module m = GetModule(moduleId, HtmlContent.FeatureGuid);
            if (m != null) { return m; }

            bool isSiteEditor = SiteUtils.UserIsSiteEditor();

            // these extra checks allow for editing an html instance from modulewrapper
            m = new Module(moduleId);
            if (
                (m.FeatureGuid != HtmlContent.FeatureGuid)
                || (m.SiteId != siteSettings.SiteId)
                || (m.ModuleId == -1)
                || ((!WebUser.IsInRoles(m.AuthorizedEditRoles)) && (!WebUser.IsAdminOrContentAdmin) && (!isSiteEditor))
                )
            { m = null; }


            return m;
        }



    }
}
