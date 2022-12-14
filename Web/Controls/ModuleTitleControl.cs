using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class ModuleTitleControl : WebControl, INamingContainer
    {
        #region Constructors

        public ModuleTitleControl()
		{
            if (HttpContext.Current == null) { return; }
			EnsureChildControls();
		}

		#endregion

        #region Control Declarations

        protected Literal litModuleTitle;
        protected HyperLink lnkModuleSettings;
        protected HyperLink lnkModuleEdit;
        protected ImageButton ibPostDraftContentForApproval;
        protected ImageButton ibApproveContent;
        protected ImageButton ibPublishContent = null;
        protected HyperLink lnkRejectContent;
        protected ImageButton ibCancelChanges;
        protected WorkflowStatusIcon statusIcon;

        #endregion

        private string literalExtraMarkup = string.Empty;
        private bool disabledModuleSettingsLink = false;
        private Module module = null;

        private string editUrl = string.Empty;
        private string editText = string.Empty;
        private bool canEdit = false;
        private bool forbidModuleSettings = false;
        private bool showEditLinkOverride = false;
        private bool enableWorkflow = false;
        private SiteModuleControl siteModule = null;
        private ContentWorkflowStatus workflowStatus = ContentWorkflowStatus.None;
        private string siteRoot = string.Empty;
        private bool isAdminEditor = false;
        private bool useHeading = true;

        private string columnId = UIHelper.CenterColumnId;
        
        #region deprecated properties

        //private string artHeader = UIHelper.ArtisteerPostMetaHeader;
        //private string artHeadingCss = UIHelper.ArtPostHeader;

        private bool useJQueryUI = false;

        public bool UseJQueryUI
        {
            get { return useJQueryUI; }
            set { useJQueryUI = value; }

        }

        private bool renderArtisteer = false;

        public bool RenderArtisteer
        {
            get { return renderArtisteer; }
            set { renderArtisteer = value; }
        }

        private bool useLowerCaseArtisteerClasses = false;

        public bool UseLowerCaseArtisteerClasses
        {
            get { return useLowerCaseArtisteerClasses; }
            set { useLowerCaseArtisteerClasses = value; }
        }

        private bool useH3ForSideHeader = false;

        public bool UseH3ForSideHeader
        {
            get { return useH3ForSideHeader; }
            set { useH3ForSideHeader = value; }
        }

        private bool useArtisteer3 = false;

        public bool UseArtisteer3
        {
            get { return useArtisteer3; }
            set { useArtisteer3 = value; }
        }

        #endregion

        private string headingTag = "h2";

        /// <summary>
        /// only used when UseModuleHeading is false
        /// </summary>
        public string Element { get; set; } = "h2";

        /// <summary>
        /// only used when UseModuleHeadingOnSideColumns is false
        /// </summary>
        public string SideColumnElement { get; set; } = "h2";



        private string topContent = string.Empty;
        private string bottomContent = string.Empty;
        private string cssClassToUse = string.Empty;

        public bool DetectSideColumn { get; set; } = false;

        /// <summary>
        /// if true (default is true) use the heading element defined on the module
        /// else use the themeable property on this control
        /// </summary>
        public bool UseModuleHeading { get; set; } = true;

        /// <summary>
        /// if true (default is true) use the heading element defined on the module
        /// else use the themeable property on this control
        /// </summary>
        public bool UseModuleHeadingOnSideColumns { get; set; } = true;

        public string LiteralExtraTopContent { get; set; } = string.Empty;

        public string LiteralExtraBottomContent { get; set; } = string.Empty;

        public string SideColumnLiteralExtraTopContent { get; set; } = string.Empty;

        public string SideColumnLiteralExtraBottomContent { get; set; } = string.Empty;

        public string ExtraCssClasses { get; set; } = string.Empty;

        public string SideColumnExtraCssClasses { get; set; } = string.Empty;

        public string LiteralHeadingTopWrap { get; set; } = string.Empty;

        public string LiteralHeadingBottomWrap { get; set; } = string.Empty;

        public string LiteraSideColumnHeadingTopWrap { get; set; } = string.Empty;

        public string LiteralSideColumnHeadingBottomWrap { get; set; } = string.Empty;

        public bool WrapLinksInSpan { get; set; } = true;

        #region Public Properties

        public Module ModuleInstance
        {
            get { return module; }
            set { module = value; }
        }

        public string LiteralExtraMarkup
        {
            get { return literalExtraMarkup; }
            set { literalExtraMarkup = value; }
        }

        public string EditUrl
        {
            get { return editUrl; }
            set { editUrl = value; }

            
        }

        public string EditText
        {
            get { return editText; }
            set { editText = value; }
           
        }

        public bool UseHeading
        {
            get { return useHeading; }
            set { useHeading = value; }

        }

        public bool DisabledModuleSettingsLink
        {
            get { return disabledModuleSettingsLink; }
            set { disabledModuleSettingsLink = value; }
        }

        public bool CanEdit
        {
            get { return canEdit; }
            set { canEdit = value; }
            
        }

        public bool IsAdminEditor
        {
            get { return isAdminEditor; }
            set { isAdminEditor = value; }

        }

        public bool ShowEditLinkOverride
        {
            get { return showEditLinkOverride; }
            set { showEditLinkOverride = value; }
            
        }

        public ContentWorkflowStatus WorkflowStatus
        {
            get { return workflowStatus; }
            set { workflowStatus = value; }
        }

        

        private bool renderEditLinksInsideHeading = true;

        public bool RenderEditLinksInsideHeading
        {
            get { return renderEditLinksInsideHeading; }
            set { renderEditLinksInsideHeading = value; }
        }

        
        private bool forceShowExtraMarkup = false;

        /// <summary>
        /// by default extra markup is only shown when IsEditable is true
        /// setting this property to true will make it show the extra markup 
        /// if there is any when the user does not have edit permission on the module
        /// </summary>
        public bool ForceShowExtraMarkup
        {
            get { return forceShowExtraMarkup; }
            set { forceShowExtraMarkup = value; }
        }

        #endregion

        private SiteModuleControl GetParentAsSiteModelControl(Control child)
        {
            if (HttpContext.Current == null || child.Parent == null) 
            { 
                return null; 
            }
            else if (child.Parent is SiteModuleControl)
            {
                return child.Parent as SiteModuleControl;
            }
            else
            {
                return GetParentAsSiteModelControl(child.Parent);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) 
            {
                writer.Write($"[{this.ID}]");
                return;
            }

            if (useHeading && topContent.Length > 0)
            {
                writer.Write(topContent);
            }

            if ((!useHeading)&&(module != null))
            {
                // only need this when not rendering a heading element
                writer.Write("<a id='module" + module.ModuleId.ToInvariantString() + "' class='moduleanchor'></a>");
            }

            if (useHeading && headingTag.Length > 0)
            {
                writer.WriteBeginTag(headingTag);

                if (module != null)
                {
                    writer.WriteAttribute("id", "module" + module.ModuleId.ToInvariantString());
                }

                writer.WriteAttribute("class", cssClassToUse + " moduletitle");
                writer.Write(HtmlTextWriter.TagRightChar);
            }

            if (useHeading && LiteralHeadingTopWrap.Length > 0)
            {
                writer.Write(LiteralHeadingTopWrap);
            }
                
            litModuleTitle.RenderControl(writer);

            if (useHeading && LiteralHeadingBottomWrap.Length > 0)
            {
                writer.Write(LiteralHeadingBottomWrap);
            }

            if (renderEditLinksInsideHeading)
            {
                if (CanEdit)
                {
                    if (WrapLinksInSpan) 
                    { 
                        writer.Write("<span class='modulelinks'>"); 
                    }
                    if (!forbidModuleSettings)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        lnkModuleSettings.RenderControl(writer);
                    }

                    if (ibCancelChanges != null && ibCancelChanges.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibCancelChanges.RenderControl(writer);
                    }

                    if (ibPostDraftContentForApproval != null && ibPostDraftContentForApproval.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibPostDraftContentForApproval.RenderControl(writer);
                    }

                    if (lnkRejectContent != null && lnkRejectContent.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        lnkRejectContent.RenderControl(writer);
                    }

                    if (ibApproveContent != null && ibApproveContent.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibApproveContent.RenderControl(writer);
                    }

                    if (ibPublishContent != null && ibPublishContent.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibPublishContent.RenderControl(writer);
                    }

                    if (statusIcon.ToolTip.Length > 0)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        statusIcon.RenderControl(writer);
                    }

                    if (
                        lnkModuleEdit != null
                        && !string.IsNullOrWhiteSpace(EditUrl)
                        && !string.IsNullOrWhiteSpace(EditText)
                    )
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        lnkModuleEdit.RenderControl(writer);
                    }

                    if (literalExtraMarkup.Length > 0)
                    {
                        writer.Write(literalExtraMarkup);
                    }

                    if (WrapLinksInSpan) 
                    { 
                        writer.Write("</span>"); 
                    }
                }
                else
                {
                    //can't edit
                    if (forceShowExtraMarkup)
                    {
                        if (literalExtraMarkup.Length > 0)
                        {
                            writer.Write(literalExtraMarkup);
                        }
                    }
                }
            }

            if (useHeading && headingTag.Length > 0)
            {
                writer.WriteEndTag(headingTag);
            }

            if (useHeading && bottomContent.Length > 0)
            {
                writer.Write(bottomContent);
            }

            if (!renderEditLinksInsideHeading)
            {
                if (CanEdit)
                {
                    writer.Write("<div class=\"edlinks\">");

                    if (!forbidModuleSettings)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        lnkModuleSettings.RenderControl(writer);
                    }

                    if (ibCancelChanges != null && ibCancelChanges.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibCancelChanges.RenderControl(writer);
                    }

                    if (ibPostDraftContentForApproval != null && ibPostDraftContentForApproval.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibPostDraftContentForApproval.RenderControl(writer);
                    }

                    if (lnkRejectContent != null && lnkRejectContent.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        lnkRejectContent.RenderControl(writer);
                    }

                    if (ibApproveContent != null && ibApproveContent.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibApproveContent.RenderControl(writer);
                    }

                    if (ibPublishContent != null && ibPublishContent.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibPublishContent.RenderControl(writer);
                    }

                    if (statusIcon.ToolTip.Length > 0)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        statusIcon.RenderControl(writer);
                    }

                    if (
                    (lnkModuleEdit != null)
                    && (!string.IsNullOrEmpty(EditUrl))
                    && (!string.IsNullOrEmpty(EditText))
                    )
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        lnkModuleEdit.RenderControl(writer);
                    }

                    writer.Write("</div>");

                } //can edit

                if (literalExtraMarkup.Length > 0)
                {
                    writer.Write(literalExtraMarkup);
                }
            }
        }

        void ibApproveContent_Click(object sender, ImageClickEventArgs e)
        {
            var siteModule = GetParentAsSiteModelControl(this);
            if (siteModule == null) { return; }
            if (!(siteModule is IWorkflow)) { return; }

            IWorkflow workflow = siteModule as IWorkflow;
            workflow.Approve();
        }

        protected void ibPostDraftContentForApproval_Click(object sender, ImageClickEventArgs e)
        {
            var siteModule = GetParentAsSiteModelControl(this);
            if (siteModule == null) { return; }
            if (!(siteModule is IWorkflow)) { return; }

            IWorkflow workflow = siteModule as IWorkflow;
            workflow.SubmitForApproval();
        }

        protected void ibCancelChanges_Click(object sender, ImageClickEventArgs e)
        {
            var siteModule = GetParentAsSiteModelControl(this);
            if (siteModule == null) { return; }
            if (!(siteModule is IWorkflow)) { return; }

            IWorkflow workflow = siteModule as IWorkflow;
            workflow.CancelChanges();
        }

       

        protected override void OnPreRender(EventArgs e)
        {
           
            base.OnPreRender(e);
            if (HttpContext.Current == null) 
            { 
                return; 
            }

            headingTag = WebConfigSettings.ModuleTitleTag;

            Initialize();

            if (DetectSideColumn)
            {
                columnId = this.GetColumnId();

                switch (columnId)
                {
                    case UIHelper.LeftColumnId:
                    case UIHelper.RightColumnId:
                        topContent = SideColumnLiteralExtraTopContent;
                        bottomContent = SideColumnLiteralExtraBottomContent;
                        cssClassToUse = SideColumnExtraCssClasses;

                        LiteralHeadingTopWrap = LiteraSideColumnHeadingTopWrap;
                        LiteralHeadingBottomWrap = LiteralSideColumnHeadingBottomWrap;

                        if (!UseModuleHeadingOnSideColumns)
                        {
                            headingTag = SideColumnElement;
                        }
                        break;

                    case UIHelper.CenterColumnId:
                    default:

                        topContent = LiteralExtraTopContent;
                        bottomContent = LiteralExtraBottomContent;
                        cssClassToUse = ExtraCssClasses;

                        if (!UseModuleHeading)
                        {
                            headingTag = Element;
                        }
                        break;
                }
            }
            else
            {
                topContent = LiteralExtraTopContent;
                bottomContent = LiteralExtraBottomContent;
                cssClassToUse = ExtraCssClasses;
                if (!UseModuleHeading)
                {
                    headingTag = Element;
                }
            }
        }

        private void Initialize()
        {
            if (HttpContext.Current == null) 
            { 
                return; 
            }
            
            siteModule = GetParentAsSiteModelControl(this);
            var useTextLinksForFeatureSettings = true;

            if (Page is mojoBasePage basePage)
            {
                useTextLinksForFeatureSettings = basePage.UseTextLinksForFeatureSettings;
            }

            if (siteModule != null)
            {
                module = siteModule.ModuleConfiguration;
                CanEdit = siteModule.IsEditable;
                enableWorkflow = siteModule.EnableWorkflow;
                forbidModuleSettings = siteModule.ForbidModuleSettings;
            }

            if (module != null)
            {
                headingTag = module.HeadElement;
                if (module.ShowTitle)
                {
                    litModuleTitle.Text = Page.Server.HtmlEncode(module.ModuleTitle);
                }
                else
                {
                    useHeading = false;
                }

                if (CanEdit)
                {
                    if (!disabledModuleSettingsLink)
                    {
                        lnkModuleSettings.Visible = true;
                        lnkModuleSettings.Text = Resource.SettingsLink;
                        lnkModuleSettings.ToolTip = Resource.ModuleEditSettings;

                        if (!useTextLinksForFeatureSettings)
                        {
                            lnkModuleSettings.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/" + WebConfigSettings.EditPropertiesImage);
                        }
                        else
                        {
                            // if its a text link make it small like the edit link
                            lnkModuleSettings.CssClass = "ModuleEditLink";
                        }

                        siteRoot = SiteUtils.GetNavigationSiteRoot();

                        lnkModuleSettings.NavigateUrl = siteRoot
                            + "/Admin/ModuleSettings.aspx?mid=" + module.ModuleId.ToInvariantString()
                            + "&pageid=" + module.PageId.ToInvariantString();

                        if ((enableWorkflow) && (siteModule != null) && (siteModule is IWorkflow))
                        {
                            SetupWorkflowControls();
                        }
                    }
                }

                if (
                    (CanEdit || ShowEditLinkOverride)
                    && EditText != null && editUrl.Length > 0)
                {
                    lnkModuleEdit.Text = EditText;
                    if (this.ToolTip.Length > 0)
                    {
                        lnkModuleEdit.ToolTip = this.ToolTip;
                    }
                    else
                    {
                        lnkModuleEdit.ToolTip = EditText;
                    }
                    lnkModuleEdit.NavigateUrl = EditUrl
                         + (EditUrl.Contains("?") ? "&" : "?")
                         + "mid=" + module.ModuleId.ToInvariantString()
                         + "&pageid=" + module.PageId.ToInvariantString();

                    if (!useTextLinksForFeatureSettings)
                    {
                        lnkModuleEdit.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/" + WebConfigSettings.EditContentImage);
                    }
                }
            }
        }

        private void SetupWorkflowControls()
        {
            if (HttpContext.Current == null) { return; }

            if (siteModule == null) { return; }
            if (module == null) { return; }

            if ((this.Page is CmsPage cmsPage) && (cmsPage.ViewMode == PageViewMode.WorkInProgress))
            {
                switch (workflowStatus)
                {
                    case ContentWorkflowStatus.Draft:

                        ibPostDraftContentForApproval.ImageUrl = Page.ResolveUrl(WebConfigSettings.RequestApprovalImage);
                        ibPostDraftContentForApproval.ToolTip = Resource.RequestApprovalToolTip;
                        ibPostDraftContentForApproval.Visible = true;

                        statusIcon.ToolTip = Resource.WorkflowDraft;

                        if (WebConfigSettings.WorkflowShowPublishForUnSubmittedDraft)
                        {
                            if (
                            (cmsPage.CurrentPage != null)
                            && (isAdminEditor || WebUser.IsInRoles(cmsPage.CurrentPage.EditRoles) || WebUser.IsInRoles(this.module.AuthorizedEditRoles)
                            || (WebConfigSettings.Use3LevelContentWorkflow && (WebUser.IsInRoles(cmsPage.CurrentPage.DraftApprovalRoles) || WebUser.IsInRoles(this.module.DraftApprovalRoles)))
                            )
                            )
                            {
                                ibApproveContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.ApproveContentImage);
                                ibApproveContent.Visible = true;
                                ibApproveContent.ToolTip = Resource.ApproveContentToolTip;
                            }
                        }
                        break;

                    case ContentWorkflowStatus.AwaitingApproval:

                        if (WebConfigSettings.Use3LevelContentWorkflow)
                        {
                            //disable edit link because draft is awaiting approval
                            lnkModuleEdit.Visible = false;
                        }

                        if (
                            (cmsPage.CurrentPage != null)
                            && (isAdminEditor || WebUser.IsInRoles(cmsPage.CurrentPage.EditRoles) || WebUser.IsInRoles(this.module.AuthorizedEditRoles)
                            || (WebConfigSettings.Use3LevelContentWorkflow && (WebUser.IsInRoles(cmsPage.CurrentPage.DraftApprovalRoles) || WebUser.IsInRoles(this.module.DraftApprovalRoles)))
                            )
                            )
                        {
                            if (WebConfigSettings.Use3LevelContentWorkflow)
                            {
                                //user can edit current draft awaiting approval
                                lnkModuleEdit.Visible = true;
                            }

                            //add in the reject and approve links:                                            
                            ibApproveContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.ApproveContentImage);
                            ibApproveContent.Visible = true;
                            ibApproveContent.ToolTip = Resource.ApproveContentToolTip;

                            lnkRejectContent.NavigateUrl =
                                siteRoot
                                + "/Admin/RejectContent.aspx?mid=" + module.ModuleId.ToInvariantString()
                                + "&pageid=" + module.PageId.ToInvariantString();

                            lnkRejectContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.RejectContentImage);
                            lnkRejectContent.ToolTip = Resource.RejectContentToolTip;
                            lnkRejectContent.Visible = true;
                        }

                        statusIcon.ToolTip = WebConfigSettings.Use3LevelContentWorkflow ? Resource.WorkflowAwaitingApproval3Level : Resource.WorkflowAwaitingApproval;
                        break;

                    case ContentWorkflowStatus.AwaitingPublishing:
                        if (
                            (cmsPage.CurrentPage != null)
                            && (isAdminEditor || WebUser.IsInRoles(cmsPage.CurrentPage.EditRoles) || WebUser.IsInRoles(this.module.AuthorizedEditRoles))
                            )
                        {
                            ibPublishContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.PublishContentImage);
                            ibPublishContent.Visible = true;
                            ibPublishContent.ToolTip = Resource.PublishContentToolTip;

                            lnkRejectContent.NavigateUrl =
                                siteRoot
                                + "/Admin/RejectContent.aspx?mid=" + module.ModuleId.ToInvariantString()
                                + "&pageid=" + module.PageId.ToInvariantString();

                            lnkRejectContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.RejectContentImage);
                            lnkRejectContent.ToolTip = Resource.RejectContentToolTip;
                            lnkRejectContent.Visible = true;
                        }

                        statusIcon.ToolTip = Resource.WorkflowAwaitingPublishing;

                        break;

                    case ContentWorkflowStatus.ApprovalRejected:
                        statusIcon.ToolTip = Resource.WorkflowRejected;
                        break;
                }

                if (
                    workflowStatus != ContentWorkflowStatus.Cancelled
                    && workflowStatus != ContentWorkflowStatus.Approved
                    && workflowStatus != ContentWorkflowStatus.None
                    )
                {
                    //allow changes to be cancelled:                                            
                    ibCancelChanges.ImageUrl = Page.ResolveUrl(WebConfigSettings.CancelContentChangesImage);
                    ibCancelChanges.ToolTip = Resource.CancelChangesToolTip;
                    ibCancelChanges.Visible = true;
                }
            }
        }


        protected override void CreateChildControls()
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            litModuleTitle = new Literal();
            
            lnkModuleSettings = new HyperLink
            {
                CssClass = "modulesettingslink"
            };
            
            lnkModuleEdit = new HyperLink
            {
                CssClass = "ModuleEditLink",
                SkinID = "plain"
            };

            ibPostDraftContentForApproval = new ImageButton
            {
                ID = "lbPostDraftContentForApproval",
                CssClass = "jqtt ModulePostDraftForApprovalLink",
                SkinID = "plain",
                Visible = false
            };
            ibPostDraftContentForApproval.Click += new ImageClickEventHandler(ibPostDraftContentForApproval_Click);
            this.Controls.Add(ibPostDraftContentForApproval);

            ibApproveContent = new ImageButton
            {
                ID = "ibApproveContent",
                CssClass = "jqtt ModuleApproveContentLink",
                SkinID = "plain",
                Visible = false
            };
            ibApproveContent.Click += new ImageClickEventHandler(ibApproveContent_Click);
            this.Controls.Add(ibApproveContent);

            if (WebConfigSettings.Use3LevelContentWorkflow)
            {
                ibPublishContent = new ImageButton
                {
                    ID = "ibPublishContent",
                    CssClass = "jqtt ModulePublishContentLink",
                    SkinID = "plain",
                    Visible = false
                };
                ibPublishContent.Click += new ImageClickEventHandler(ibApproveContent_Click); //approve and publish are the same at this point so we have only one method
                this.Controls.Add(ibPublishContent);
            }

            lnkRejectContent = new HyperLink
            {
                ID = "ibRejectContent",
                CssClass = "jqtt ModuleRejectContentLink",
                SkinID = "plain",
                Visible = false
            };

            ibCancelChanges = new ImageButton
            {
                ID = "ibCancelChanges",
                CssClass = "jqtt ModuleCancelChangesLink",
                SkinID = "plain",
                Visible = false
            };
            UIHelper.AddConfirmationDialog(ibCancelChanges, Resource.CancelContentChangesButtonWarning);
            ibCancelChanges.Click += new ImageClickEventHandler(ibCancelChanges_Click);
            this.Controls.Add(ibCancelChanges);

            statusIcon = new WorkflowStatusIcon();
            this.Controls.Add(statusIcon);
        }
    }
}
