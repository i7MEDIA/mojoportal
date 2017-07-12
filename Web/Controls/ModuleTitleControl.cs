///	Author:					
///	Created:				2006-12-14
/// Last Modified:			2013-08-29

using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    public class ModuleTitleControl : WebControl, INamingContainer
    {
        #region Constructors

        public ModuleTitleControl()
		{
            //if (this.Site != null && this.Site.DesignMode) 
            //{
            //    this.Visible = false;
            //    return; 
            //}
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
        //protected ClueTipHelpLink statusLink;
        protected WorkflowStatusIcon statusIcon;

        #endregion

        private string literalExtraMarkup = string.Empty;
        private bool disabledModuleSettingsLink = false;
        private Module module = null;

        private string editUrl = string.Empty;
        private string editText = string.Empty;
        //private bool useHTag = true;
        private bool canEdit = false;
        private bool forbidModuleSettings = false;
        private bool showEditLinkOverride = false;
        private bool enableWorkflow = false;
        private SiteModuleControl siteModule = null;
        private ContentWorkflowStatus workflowStatus = ContentWorkflowStatus.None;
        private string siteRoot = string.Empty;
        private bool isAdminEditor = false;
        private bool useHeading = true;

        #region deprecated properties

        private string columnId = UIHelper.CenterColumnId;
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

        private string element = "h2";

        /// <summary>
        /// only used when UseModuleHeading is false
        /// </summary>
        public string Element
        {
            get { return element; }
            set { element = value; }
        }

        private string sideColumnElement = "h2";

        /// <summary>
        /// only used when UseModuleHeadingOnSideColumns is false
        /// </summary>
        public string SideColumnElement
        {
            get { return sideColumnElement; }
            set { sideColumnElement = value; }
        }



        private string topContent = string.Empty;
        private string bottomContent = string.Empty;
        private string cssClassToUse = string.Empty;


        private bool detectSideColumn = false;

        public bool DetectSideColumn
        {
            get { return detectSideColumn; }
            set { detectSideColumn = value; }
        }

        private bool useModuleHeading = true;

        /// <summary>
        /// if true (default is true) use the heading element defined on the module
        /// else use the themeable property on this control
        /// </summary>
        public bool UseModuleHeading
        {
            get { return useModuleHeading; }
            set { useModuleHeading = value; }
        }

        private bool useModuleHeadingOnSideColumns = true;

        /// <summary>
        /// if true (default is true) use the heading element defined on the module
        /// else use the themeable property on this control
        /// </summary>
        public bool UseModuleHeadingOnSideColumns
        {
            get { return useModuleHeadingOnSideColumns; }
            set { useModuleHeadingOnSideColumns = value; }
        }

        private string literalExtraTopContent = string.Empty;

        public string LiteralExtraTopContent
        {
            get { return literalExtraTopContent; }
            set { literalExtraTopContent = value; }
        }

        private string literalExtraBottomContent = string.Empty;

        public string LiteralExtraBottomContent
        {
            get { return literalExtraBottomContent; }
            set { literalExtraBottomContent = value; }
        }

        private string sideColumnLiteralExtraTopContent = string.Empty;

        public string SideColumnLiteralExtraTopContent
        {
            get { return sideColumnLiteralExtraTopContent; }
            set { sideColumnLiteralExtraTopContent = value; }
        }

        private string sideColumnLiteralExtraBottomContent = string.Empty;

        public string SideColumnLiteralExtraBottomContent
        {
            get { return sideColumnLiteralExtraBottomContent; }
            set { sideColumnLiteralExtraBottomContent = value; }
        }

        private string extraCssClasses = string.Empty;

        public string ExtraCssClasses
        {
            get { return extraCssClasses; }
            set { extraCssClasses = value; }
        }

        private string sideColumnExtraCssClasses = string.Empty;

        public string SideColumnExtraCssClasses
        {
            get { return sideColumnExtraCssClasses; }
            set { sideColumnExtraCssClasses = value; }
        }

        private string literalHeadingTopWrap = string.Empty;

        public string LiteralHeadingTopWrap
        {
            get { return literalHeadingTopWrap; }
            set { literalHeadingTopWrap = value; }
        }

        private string literalHeadingBottomWrap = string.Empty;

        public string LiteralHeadingBottomWrap
        {
            get { return literalHeadingBottomWrap; }
            set { literalHeadingBottomWrap = value; }
        }

        private string literaSideColumnlHeadingTopWrap = string.Empty;

        public string LiteraSideColumnlHeadingTopWrap
        {
            get { return literaSideColumnlHeadingTopWrap; }
            set { literaSideColumnlHeadingTopWrap = value; }
        }

        private string literalSideColumnHeadingBottomWrap = string.Empty;

        public string LiteralSideColumnHeadingBottomWrap
        {
            get { return literalSideColumnHeadingBottomWrap; }
            set { literalSideColumnHeadingBottomWrap = value; }
        }

        private bool wrapLinksInSpan = true;

        public bool WrapLinksInSpan
        {
            get { return wrapLinksInSpan; }
            set { wrapLinksInSpan = true; }
        }

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
            if (HttpContext.Current == null) { return null; }

            if (child.Parent == null)
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
                writer.Write("[" + this.ID + "]");
                return;
            }
            
            //if ((useHeading)&&(renderArtisteer))
            //{
            //    string firstClass = string.Empty;
            //    if ((artHeader == UIHelper.ArtisteerBlockHeader) || (artHeader == UIHelper.ArtisteerBlockHeaderLower))
            //    {
            //        firstClass = "art-bar ";
            //    }

            //    writer.Write("<div class=\"" + firstClass + artHeader + "\">\n");

            //    if ((artHeader == UIHelper.ArtisteerBlockHeader)||(artHeader == UIHelper.ArtisteerBlockHeaderLower))
            //    {
            //        writer.Write("<div class=\"l\"></div>");
            //        writer.Write("<div class=\"r\"></div>");
            //        writer.Write("<div class=\"art-header-tag-icon\">");
            //        if (!useArtisteer3) { writer.Write("<div class=\"t\">"); }
            //    }

            //}
            //else if ((useJQueryUI)&&(module != null) &&(module.ShowTitle))
            //{
            //    writer.Write("<div class=\"ui-widget-header ui-corner-top\">");
            //}

            if ((useHeading) && (topContent.Length > 0))
            {
                writer.Write(topContent);
            }

                
            if ((!useHeading)&&(module != null))
            {
                // only need this when not rendering a heading element
                writer.Write("<a id='module" + module.ModuleId.ToInvariantString() + "' class='moduleanchor'></a>");
            }

                

            if ((useHeading)&&(headingTag.Length > 0))
            {
                writer.WriteBeginTag(headingTag);

                if (module != null)
                {
                    writer.WriteAttribute("id", "module" + module.ModuleId.ToInvariantString());
                }

                //if (useArtisteer3)
                //{
                //    writer.WriteAttribute("class", artHeadingCss + " t moduletitle");
                //}
                //else
                //{
                //    writer.WriteAttribute("class", artHeadingCss + " moduletitle");
                //}

                writer.WriteAttribute("class", cssClassToUse + " moduletitle");

                writer.Write(HtmlTextWriter.TagRightChar);
            }

            if ((useHeading) &&(literalHeadingTopWrap.Length > 0))
            {
                writer.Write(literalHeadingTopWrap);
            }
                
            litModuleTitle.RenderControl(writer);

            if ((useHeading) &&(literalHeadingBottomWrap.Length > 0))
            {
                writer.Write(literalHeadingBottomWrap);
            }

            if (renderEditLinksInsideHeading)
            {
                if (CanEdit)
                {
                    if (wrapLinksInSpan) { writer.Write("<span class='modulelinks'>"); }
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

                    //joe davis
                    if (ibPublishContent != null && ibPublishContent.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibPublishContent.RenderControl(writer);
                    }

                    //if (statusLink != null && statusLink.Visible)
                    //{
                    if (statusIcon.ToolTip.Length > 0)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        //statusLink.ToolTip = Resource.WorkflowStatus;
                        //statusLink.RenderControl(writer);

                        statusIcon.RenderControl(writer);
                    }
                    //}

                    if (
                    (lnkModuleEdit != null)
                        && (!string.IsNullOrEmpty(EditUrl))
                    && (!string.IsNullOrEmpty(EditText))
                    )
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        lnkModuleEdit.RenderControl(writer);
                    }

                    if (literalExtraMarkup.Length > 0)
                    {
                        writer.Write(literalExtraMarkup);
                    }

                    if (wrapLinksInSpan) { writer.Write("</span>"); }
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

            if ((useHeading)&&(headingTag.Length > 0))
            {
                writer.WriteEndTag(headingTag);
            }

            //if ((useHeading) && (renderArtisteer))
            //{
            //    writer.Write("</div>");
            //    if ((artHeader == UIHelper.ArtisteerBlockHeader) || (artHeader == UIHelper.ArtisteerBlockHeaderLower))
            //    {
            //        writer.Write("</div>");
            //        if (!useArtisteer3) { writer.Write("</div>"); }
            //    }
            //}
            //else if ((useJQueryUI)&&(module != null) &&(module.ShowTitle))
            //{
            //    writer.Write("</div>");
            //}

            if ((useHeading) &&(bottomContent.Length > 0))
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

                    //joe davis
                    if (ibPublishContent != null && ibPublishContent.Visible)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        ibPublishContent.RenderControl(writer);
                    }

                    //if (statusLink != null && statusLink.Visible)
                    //{
                    if (statusIcon.ToolTip.Length > 0)
                    {
                        writer.Write(HtmlTextWriter.SpaceChar);
                        //statusLink.ToolTip = Resource.WorkflowStatus;
                        //statusLink.RenderControl(writer);
                        statusIcon.RenderControl(writer);
                    }
                    //}

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
            SiteModuleControl siteModule = GetParentAsSiteModelControl(this);
            if (siteModule == null) { return; }
            if (!(siteModule is IWorkflow)) { return; }

            IWorkflow workflow = siteModule as IWorkflow;
            workflow.Approve();

        }

        protected void ibPostDraftContentForApproval_Click(object sender, ImageClickEventArgs e)
        {
            SiteModuleControl siteModule = GetParentAsSiteModelControl(this);
            if (siteModule == null) { return; }
            if (!(siteModule is IWorkflow)) { return; }

            IWorkflow workflow = siteModule as IWorkflow;
            workflow.SubmitForApproval();
           
        }

        protected void ibCancelChanges_Click(object sender, ImageClickEventArgs e)
        {
            SiteModuleControl siteModule = GetParentAsSiteModelControl(this);
            if (siteModule == null) { return; }
            if (!(siteModule is IWorkflow)) { return; }

            IWorkflow workflow = siteModule as IWorkflow;
            workflow.CancelChanges();
           
        }

       

        protected override void OnPreRender(EventArgs e)
        {
           
            base.OnPreRender(e);
            if (HttpContext.Current == null) { return; }

            headingTag = WebConfigSettings.ModuleTitleTag;

            Initialize();

            //if ((useHeading) && (renderArtisteer))
            if (detectSideColumn)
            {
                columnId = this.GetColumnId();

                //if (useLowerCaseArtisteerClasses)
                //{
                //    artHeader = UIHelper.ArtisteerPostMetaHeaderLower;
                //    artHeadingCss = UIHelper.ArtPostHeaderLower;

                //}

                switch (columnId)
                {
                    case UIHelper.LeftColumnId:
                    case UIHelper.RightColumnId:

                        //if (useLowerCaseArtisteerClasses)
                        //{
                        //    if ((artHeader == UIHelper.ArtisteerPostMetaHeader)||(artHeader == UIHelper.ArtisteerPostMetaHeaderLower))
                        //    {
                        //        artHeader = UIHelper.ArtisteerBlockHeaderLower;
                        //    }
                        //}
                        //else
                        //{
                        //    if (artHeader == UIHelper.ArtisteerPostMetaHeader)
                        //    {
                        //        artHeader = UIHelper.ArtisteerBlockHeader;
                        //    }
                        //}

                        //artHeadingCss = string.Empty;

                        //if (useH3ForSideHeader) { headingTag = "h3"; }

                        topContent = sideColumnLiteralExtraTopContent;
                        bottomContent = sideColumnLiteralExtraBottomContent;
                        cssClassToUse = sideColumnExtraCssClasses;

                        literalHeadingTopWrap = literaSideColumnlHeadingTopWrap;
                        literalHeadingBottomWrap = literalSideColumnHeadingBottomWrap;

                        if (!UseModuleHeadingOnSideColumns)
                        {
                            headingTag = sideColumnElement;
                        }

                        break;

                    case UIHelper.CenterColumnId:
                    default:

                        topContent = literalExtraTopContent;
                        bottomContent = literalExtraBottomContent;
                        cssClassToUse = extraCssClasses;

                        if (!UseModuleHeading)
                        {
                            headingTag = element;
                        }

                        break;

                }
            }
            else
            {
                topContent = literalExtraTopContent;
                bottomContent = literalExtraBottomContent;
                cssClassToUse = extraCssClasses;
                if (!UseModuleHeading)
                {
                    headingTag = element;
                }
            }
            
           
            

        }

        private void Initialize()
        {
            if (HttpContext.Current == null) { return; }
            
            siteModule = GetParentAsSiteModelControl(this);

            bool useTextLinksForFeatureSettings = true;
            mojoBasePage basePage = Page as mojoBasePage;
            if (basePage != null)
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
                    ((CanEdit) || (ShowEditLinkOverride))
                    && ((EditText != null) && (editUrl.Length > 0)))
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

           
            CmsPage cmsPage = this.Page as CmsPage;
            if ((cmsPage != null) && (cmsPage.ViewMode == PageViewMode.WorkInProgress))
            {
                //ScriptReference script = new ScriptReference();
                //script.Path = "~/ClientScript/jqmojo/jquery.cluetip.js";
                //cmsPage.ScriptConfig.AddPathScriptReference(script);

                //this.Controls.Add(statusLink);

                switch (workflowStatus)
                {
                    case ContentWorkflowStatus.Draft:

                        ibPostDraftContentForApproval.ImageUrl = Page.ResolveUrl(WebConfigSettings.RequestApprovalImage);
                        ibPostDraftContentForApproval.ToolTip = Resource.RequestApprovalToolTip;
                        ibPostDraftContentForApproval.Visible = true;
                        //statusLink.HelpKey = "workflowstatus-draft-help";

                        statusIcon.ToolTip = Resource.WorkflowDraft;
                        //statusLink.Visible = true;
                        //statusLink.HookupScript();

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
                            //joe davis
                            //disable edit link because draft is awaiting approval
                            lnkModuleEdit.Visible = false;
                        }

                        //if (WebUser.IsAdminOrContentAdminOrContentPublisher)
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

                        //statusLink.Visible = true;
                        //statusLink.HookupScript();

                        break;

                    case ContentWorkflowStatus.AwaitingPublishing:
                        //joe davis
                        if (
                            (cmsPage.CurrentPage != null)
                            && (isAdminEditor || WebUser.IsInRoles(cmsPage.CurrentPage.EditRoles) || WebUser.IsInRoles(this.module.AuthorizedEditRoles))
                            )
                        {

                            //add in the reject and publish links:                                            
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
                       // statusLink.HelpKey = "workflowstatus-rejected-help";
                        statusIcon.ToolTip = Resource.WorkflowRejected;
                        //statusLink.Visible = true;
                        //statusLink.HookupScript();
                        break;

                    
                }

                if (
                    (workflowStatus != ContentWorkflowStatus.Cancelled)
                    && (workflowStatus != ContentWorkflowStatus.Approved)
                    && (workflowStatus != ContentWorkflowStatus.None)
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
            if (HttpContext.Current == null) { return; }

            litModuleTitle = new Literal();
            //this.Controls.Add(litModuleTitle);
            lnkModuleSettings = new HyperLink();
            lnkModuleSettings.CssClass = "modulesettingslink";
            //this.Controls.Add(lnkModuleSettings);
            
            lnkModuleEdit = new HyperLink();
            //this.Controls.Add(lnkModuleEdit);
            lnkModuleEdit.CssClass = "ModuleEditLink";
            lnkModuleEdit.SkinID = "plain";

            

            ibPostDraftContentForApproval = new ImageButton();
            ibPostDraftContentForApproval.ID = "lbPostDraftContentForApproval";
            ibPostDraftContentForApproval.CssClass = "jqtt ModulePostDraftForApprovalLink";
            ibPostDraftContentForApproval.SkinID = "plain";
            ibPostDraftContentForApproval.Visible = false;
            ibPostDraftContentForApproval.Click += new ImageClickEventHandler(ibPostDraftContentForApproval_Click);
            this.Controls.Add(ibPostDraftContentForApproval);

            ibApproveContent = new ImageButton();
            ibApproveContent.ID = "ibApproveContent";
            ibApproveContent.CssClass = "jqtt ModuleApproveContentLink";
            ibApproveContent.SkinID = "plain";
            ibApproveContent.Visible = false;
            ibApproveContent.Click += new ImageClickEventHandler(ibApproveContent_Click);
            this.Controls.Add(ibApproveContent);

            if (WebConfigSettings.Use3LevelContentWorkflow)
            {
                //joe davis
                ibPublishContent = new ImageButton();
                ibPublishContent.ID = "ibPublishContent";
                ibPublishContent.CssClass = "jqtt ModulePublishContentLink";
                ibPublishContent.SkinID = "plain";
                ibPublishContent.Visible = false;
                ibPublishContent.Click += new ImageClickEventHandler(ibApproveContent_Click); //approve and publish are the same at this point so we have only one method
                this.Controls.Add(ibPublishContent);
            }

            lnkRejectContent = new HyperLink();
            lnkRejectContent.ID = "ibRejectContent";
            lnkRejectContent.CssClass = "jqtt ModuleRejectContentLink";
            lnkRejectContent.SkinID = "plain";
            lnkRejectContent.Visible = false;

            ibCancelChanges = new ImageButton();
            ibCancelChanges.ID = "ibCancelChanges";
            ibCancelChanges.CssClass = "jqtt ModuleCancelChangesLink";
            ibCancelChanges.SkinID = "plain";
            ibCancelChanges.Visible = false;
            UIHelper.AddConfirmationDialog(ibCancelChanges, Resource.CancelContentChangesButtonWarning);
            ibCancelChanges.Click += new ImageClickEventHandler(ibCancelChanges_Click);
            this.Controls.Add(ibCancelChanges);

            statusIcon = new WorkflowStatusIcon();
            //statusLink = new ClueTipHelpLink();
            //if (!HttpContext.Current.Request.IsAuthenticated)
            //{
            //    statusLink.AssumeScriptIsLoaded = true; //we only show the cluetip if the user is in an edit role, so leave out the script when it isn't needed
            //}
            //statusLink.Visible = false;

            //this.Controls.Add(statusLink);
            this.Controls.Add(statusIcon);
           

        }

        

    }
}
